using Microsoft.Azure.Cosmos.Table;
using System;

namespace BabaFunkeEmailManager.Data.Entities
{
    public class SubscriberEntity : TableEntity
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string SubCategory { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime SubscribedOn { get; set; }
        public DateTime? UnsubscribedOn { get; set; }
    }
}