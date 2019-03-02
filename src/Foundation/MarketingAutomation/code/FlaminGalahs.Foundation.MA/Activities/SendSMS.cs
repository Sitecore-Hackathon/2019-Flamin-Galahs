using System;
using Sitecore.XConnect.Collection.Model;
using Sitecore.Xdb.MarketingAutomation.Core.Processing.Plan;
using Sitecore.Framework.Conditions;
using System.Collections.Generic;
using Sitecore.Xdb.MarketingAutomation.Core.Activity;
using FlaminGalahs.Foundation.MA.Services;

namespace FlaminGalahs.Foundation.MA.Extensions
{
    public class SendSMS : IActivity
    {
        // Parameters
        public ISMSService SMSService { get; set; }

        public IActivityServices Services { get; set; }
        
        public SendSMS(ISMSService smsService)
        {
            SMSService = smsService;
        }

        public ActivityResult Invoke(IContactProcessingContext context)
        {
            Condition.Requires(context.Contact).IsNotNull();

            if (context.Contact.PhoneNumbers() != null && !string.IsNullOrWhiteSpace(context.Contact.PhoneNumbers().PreferredPhoneNumber?.Number))
            {
                var phoneNumber = context.Contact.PhoneNumbers().PreferredPhoneNumber.Number; // Ignoring country code and extension for now

                var message = "Quote reminder from Flamin Galahs";

                SMSService.SendSMS(phoneNumber, message);
                
                return new SuccessMove("true");
            }

            return new SuccessMove("false");
        }
    }
}