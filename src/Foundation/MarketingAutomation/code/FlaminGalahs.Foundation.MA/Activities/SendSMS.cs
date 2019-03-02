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

        // Custom service

        public SendSMS(ISMSService smsService)
        {
            SMSService = smsService;
        }

        public ActivityResult Invoke(IContactProcessingContext context)
        {
            Condition.Requires(context.Contact).IsNotNull();
            string preferredMail = String.Empty;

            if (context.Contact.Emails() != null && !context.Contact.ConsentInformation().DoNotMarket)
            {
                preferredMail = context.Contact.Emails().PreferredEmail.SmtpAddress;

                string message = "Quote reminder from Flamin Galahs";

                // Change e-mail subject depending on job title
                if (context.Contact.Personal() != null)
                {
                    if (SMSService.IsJobInCategory(context.Contact.Personal().JobTitle, JobService.DeveloperCategory))
                    {
                        subject = "Geek out with us at Symposium!";
                    }
                    else if (JobService.IsJobInCategory(context.Contact.Personal().JobTitle, JobService.ExecutiveCategory))
                    {
                        subject = "Magnify your marketing with Sitecore Experience Cloud";
                    }
                }

                SMSService.SendSMS("0421348380",message);

                // Move to "true" path
                return new SuccessMove("true");
            }

            // Move to "false" path
            return new SuccessMove("false");
        }
    }
}