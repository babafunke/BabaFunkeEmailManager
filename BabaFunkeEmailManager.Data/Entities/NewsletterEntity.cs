using Microsoft.Azure.Cosmos.Table;
using System;

namespace BabaFunkeEmailManager.Data.Entities
{
    public class NewsletterEntity : TableEntity
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsEnabled { get; set; }
    }
}