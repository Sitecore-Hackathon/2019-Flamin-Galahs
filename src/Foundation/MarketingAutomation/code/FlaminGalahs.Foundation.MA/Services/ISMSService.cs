using System.Collections.Generic;

namespace FlaminGalahs.Foundation.MA.Services
{
    public interface ISMSService
    {
        void SendSMS(string mobileNumber, string message);
    }

    public class SMSService : ISMSService
    {
        public void SendSMS(string mobileNumber, string message)
        {
            // E-mail sending logic here
        }
    }
}