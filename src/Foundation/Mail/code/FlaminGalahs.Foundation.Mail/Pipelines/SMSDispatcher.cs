using FlaminGalahs.Foundation.FGSMS.Services;
using FlaminGalahs.Foundation.Mail.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Configuration;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.EDS.Core.Dispatch;
using Sitecore.EmailCampaign.Cm.Pipelines.SendEmail;
using Sitecore.EmailCampaign.Model.Web.Exceptions;
using Sitecore.EmailCampaign.Model.Web.Settings;
using Sitecore.ExM.Framework.Diagnostics;
using Sitecore.Modules.EmailCampaign;
using Sitecore.Pipelines;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace FlaminGalahs.Foundation.Mail.Pipelines
{
    public class SMSDispatcher : Sitecore.EmailCampaign.Cm.Pipelines.SendEmail.SendEmail
    {
        private readonly Random _rand = new Random();
        private readonly ILogger _logger;
        private readonly IDispatchManager _dispatchManager;
        private readonly EcmSettings _exmSettings;

        public SMSDispatcher(ILogger logger)
          : this(logger, Factory.CreateObject("exm/eds/dispatchManager", true) as IDispatchManager, ServiceLocator.ServiceProvider.GetService<EcmSettings>())
        {
        }

        public SMSDispatcher(ILogger logger, IDispatchManager dispatchManager, EcmSettings exmSettings) : base (logger,dispatchManager,exmSettings)
        {
            Assert.ArgumentNotNull((object)logger, nameof(logger));
            Assert.ArgumentNotNull((object)dispatchManager, nameof(dispatchManager));
            Assert.ArgumentNotNull((object)exmSettings, nameof(exmSettings));
            this._logger = logger;
            this._dispatchManager = dispatchManager;
            this._exmSettings = exmSettings;
        }

        public void Process(SendMessageArgs args)
        {
            //If the message is a SMS, we send it here, otherwise we let base handle
                    var sms = args.EcmMessage as SMSMessageType;
            if (sms == null)
            {
                base.Process(args);
                return;
            }

            //It's an SMS
            var message = args.CustomData["EmailMessage"] as EmailMessage;
            if (message.Recipients.Count < 1)
            {
                args.AddMessage("Missing Recipients from EmailMessage argument.");
                return;
            }

            args.StartSendTime = DateTime.UtcNow;

            var from = message.Subject;
            var phoneNumber = sms.PersonalizationRecipient.Identifiers.FirstOrDefault(x => x.Source == "mobile").Identifier;

            var text = WebUtility.HtmlDecode(message.PlainTextBody);

            try
            {
                //TODO:  DI this.
                var service = new SMSService();
                service.SendSMS(phoneNumber, text);
                //DO STUFF IN HERE TO SEND SMS
                base.Process(args);
            }
            catch (Exception ex)
            {
                throw new NonCriticalException(ex.Message, ex);
            }

            args.AddMessage("ok", PipelineMessageType.Information);
            args.SendingTime = Util.GetTimeDiff(args.StartSendTime, DateTime.UtcNow);
        }
    }
}