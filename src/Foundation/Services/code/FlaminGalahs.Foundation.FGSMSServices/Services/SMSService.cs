using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace FlaminGalahs.Foundation.FGSMS.Services
{
    public class SMSService : ISMSService
    {
        public void SendSMS(string mobileNumber, string message)
        {
            string accountSid = Sitecore.Configuration.Settings.GetSetting("FlaminGalahs.Foundation.MA.Twilio.Sid");
            string authToken = Sitecore.Configuration.Settings.GetSetting("FlaminGalahs.Foundation.MA.Twilio.AuthToken");

            TwilioClient.Init(accountSid, authToken);

            var messageResource = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(Sitecore.Configuration.Settings.GetSetting("FlaminGalahs.Foundation.MA.Twilio.FromNumber")), // TODO Confirm if we need a from number
                to: new Twilio.Types.PhoneNumber(mobileNumber) // TODO Confirm if format is ok. Example from doco was "+15558675310"
            );
        }
    }
}