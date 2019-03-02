namespace FlaminGalahs.Foundation.MA.Services
{
    /// <summary>
    /// SMS service.
    /// </summary>
    public interface ISMSService
    {
        /// <summary>
        /// Sends an SMS
        /// </summary>
        /// <param name="mobileNumber">The mobile number to send the message to.</param>
        /// <param name="message">The message</param>
        void SendSMS(string mobileNumber, string message);
    }
}