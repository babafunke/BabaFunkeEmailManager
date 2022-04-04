using Microsoft.Azure.Cosmos.Table;
using System;

namespace BabaFunkeEmailManager.Data.Entities
{
    public class RequestHeaderEntity : TableEntity
    {
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string NewsletterId { get; set; }
    }
}