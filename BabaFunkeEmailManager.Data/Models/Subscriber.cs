using System;

namespace BabaFunkeEmailManager.Data.Models
{
    /// <summary>
    /// A Person or Contact that will be the recipient of the newsletter emails.
    /// </summary>
    public class Subscriber
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Category { get; set; } = "Subscriber";
        public string SubCategory { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime SubscribedOn { get; set; }
        public DateTime? UnsubscribedOn { get; set; }
    }
}