using System;

namespace BabaFunkeEmailManager.Data.Models
{
    /// <summary>
    /// This represents the message that will be sent out to Subscribers via email.
    /// </summary>
    public class Newsletter
    {
        public string NewsletterId { get; set; }
        public string Category { get; set; } = "Newsletter";
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
    }
}