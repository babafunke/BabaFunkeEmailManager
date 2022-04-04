using System;

namespace BabaFunkeEmailManager.Data.Models
{
    /// <summary>
    /// This represents the response from our Api to a request.
    /// </summary>
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime SentOn { get; set; }

        public static ServiceResponse<T> GetServiceResponse(string message, T data, bool isSuccess = false)
        {
            return new ServiceResponse<T>
            {
                Data = data,
                IsSuccess = isSuccess,
                Message = message,
                SentOn = DateTime.Now
            };
        }
    }
}