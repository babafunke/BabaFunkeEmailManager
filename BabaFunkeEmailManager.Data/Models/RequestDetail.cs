namespace BabaFunkeEmailManager.Data.Models
{
    /// <summary>
    /// This is a specific object detail for each item captured in a RequestHeader
    /// </summary>
    public class RequestDetail
    {
        public RequestDetail(string requestHeaderId, string emailBody, string emailSubject, string subscriberEmail, string subscriberFirstname)
        {
            RequestHeaderId = requestHeaderId;
            EmailBody = emailBody;
            EmailSubject = emailSubject;
            SubscriberEmail = subscriberEmail;
            SubscriberFirstname = subscriberFirstname;
        }

        public string RequestHeaderId { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string SubscriberEmail { get; set; }
        public string SubscriberFirstname { get; set; }
    }
}