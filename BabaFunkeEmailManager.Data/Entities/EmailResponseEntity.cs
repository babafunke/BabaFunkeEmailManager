using Microsoft.Azure.Cosmos.Table;
using System;

namespace BabaFunkeEmailManager.Data.Entities
{
    public class EmailResponseEntity : TableEntity
    {
        public EmailResponseEntity() { }

        public EmailResponseEntity(string partitionKey) : base(partitionKey, CreateRowKey())
        {
            partitionKey = PartitionKey;
        }

        public bool Status { get; set; }
        public string Email { get; set; }
        public string ErrorMessages { get; set; }
        public static string CreateRowKey() => Guid.NewGuid().ToString();
    }
}