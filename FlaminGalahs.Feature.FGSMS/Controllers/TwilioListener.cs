using Sitecore.UniversalTrackerClient.Entities;
using Sitecore.UniversalTrackerClient.Request.RequestBuilder;
using Sitecore.UniversalTrackerClient.Session.SessionBuilder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;

namespace FlaminGalahs.Feature.FGSMS
{
    public class TwilioListenerController : ApiController
    {
        [HttpGet]
        public dynamic Ping()
        {
            return "OK";
        }

        [HttpPost]
        public async Task<dynamic> Incoming(SmsRequest incomingMessage)
        {
            if (incomingMessage == null)
                return NotFound();

            await SendInteraction(incomingMessage);

            return incomingMessage.Body;
        }

        private async Task<bool> SendInteraction(SmsRequest incomingMessage)
        {
            //TODO: Move to config
            string instanceUrl = Sitecore.Configuration.Settings.GetSetting("FGSMS.UTInstanceURL");
            string channelId = Sitecore.Configuration.Settings.GetSetting("FGSMS.UTChannelId"); ;
            string definitionId = Sitecore.Configuration.Settings.GetSetting("FGSMS.UTEventDefinitionId");
            string contactIdentifierName = Sitecore.Configuration.Settings.GetSetting("FGSMS.ContactIdentifierName");
            string apiUserAgent = Sitecore.Configuration.Settings.GetSetting("FGSMS.APIUserAgent");
            string customValueBodyKey = Sitecore.Configuration.Settings.GetSetting("FGSMS.CustomValueBodyKey");


            var defaultInteraction = UTEntitiesBuilder.Interaction()
                                            .ChannelId(channelId)
                                            .Initiator(InteractionInitiator.Contact)
                                            .Contact(contactIdentifierName, incomingMessage.From)
                                            .UserAgent(apiUserAgent)
                                            .Build();

            using (var session = SitecoreUTSessionBuilder.SessionWithHost(instanceUrl)
                                    .DefaultInteraction(defaultInteraction)
                                    .BuildSession())
            {

                var timestamp = DateTime.Now;
                var eventRequest = UTRequestBuilder.EventWithDefenitionId(definitionId)
                .Timestamp(timestamp)
               .Build();

                await session.TrackEventAsync(eventRequest);

                if (incomingMessage.Body.ToUpper().StartsWith("QUOTE"))
                {
                    var goalRequest = UTRequestBuilder.GoalEvent("1640AD87-4087-4CBA-8A62-7D8062877E78", timestamp)
                   
                   .AddCustomValues(new Dictionary<string, string>{
                                        { customValueBodyKey, incomingMessage.Body }
                   }).Build();
                    await session.TrackGoalAsync(goalRequest);
                }
                else if (incomingMessage.Body.ToUpper().StartsWith("BUY"))
                {
                    var goalRequest = UTRequestBuilder.GoalEvent("644AC72C-A006-4819-BE52-EE3ECA4FB54E", timestamp)
                 
                  .AddCustomValues(new Dictionary<string, string>{
                                        { customValueBodyKey, incomingMessage.Body }
                  }).Build();
                    await session.TrackGoalAsync(goalRequest);

                }

                await session.CompleteCurrentInteractionAsync();
            }
            return true;

        }
    }
}