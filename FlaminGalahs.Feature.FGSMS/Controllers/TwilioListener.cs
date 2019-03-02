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
                string instanceUrl = "https://sc91.utcollection";
                string channelId = "DC8E7268-BDBA-4545-A703-CCD0775A86BD";
                string definitionId = "28A7C944-B8B6-45AD-A635-6F72E8F81F69";
                var defaultInteraction = UTEntitiesBuilder.Interaction()
                                                           .ChannelId(channelId)
                                                           .Initiator(InteractionInitiator.Contact)
                                                           .Contact("mobile", incomingMessage.From)
                                                           .UserAgent("Twilio listener")
                                                           .Build();

                using (var session = SitecoreUTSessionBuilder.SessionWithHost(instanceUrl)
                                                        .DefaultInteraction(defaultInteraction)
                                                        .BuildSession()
                        )
                {
                    var goalRequest = UTRequestBuilder.GoalEvent(definitionId, DateTime.Now)
                    .AddCustomValues(new Dictionary<string, string>{
                        { "Body", incomingMessage.Body }}).Build();

                var eventResponse = await session.TrackGoalAsync(goalRequest);
                    await session.CompleteCurrentInteractionAsync();
                   
                }
                return true;

        }
    }
}