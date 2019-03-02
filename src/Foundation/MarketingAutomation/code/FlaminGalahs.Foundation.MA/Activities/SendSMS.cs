using Sitecore.XConnect.Collection.Model;
using Sitecore.Xdb.MarketingAutomation.Core.Processing.Plan;
using Sitecore.Framework.Conditions;
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

                var message = "You recently requested a quote from the Flamin Galahs. Would you like to receive a discount on your quoted price? Reply with Yes or No"; // TODO Make configurable, preferably in a MA plan

                SMSService.SendSMS(phoneNumber, message);
                
                return new SuccessMove("true");
            }

            return new SuccessMove("false");
        }
    }
}