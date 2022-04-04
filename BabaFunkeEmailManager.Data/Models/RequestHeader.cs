using System;

namespace BabaFunkeEmailManager.Data.Models
{
    /// <summary>
    /// This represents the Unit of Work for the request to send out a newsletter.
    /// Each RequestHeader will the details of recipients and newsletter to send.
    /// </summary>
    public class RequestHeader
    {
        public string RequestHeaderId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public string NewsletterId { get; set; }
        public string SubscriberSubCategory { get; set; }
    }
}