using System;

namespace BabaFunkeEmailManager.Data.Models
{
    /// <summary>
    /// The response object returned for every sent emails
    /// </summary>
    public class EmailResponse
    {
        public string Id { get; set; }
        public string RequestHeaderId { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
        public string ErrorMessages { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}