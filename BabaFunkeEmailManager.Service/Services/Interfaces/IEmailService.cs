using BabaFunkeEmailManager.Data.Models;
using FluentEmail.Core.Models;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Services.Interfaces
{
    /// <summary>
    /// The email sending service interface
    /// </summary>
    public interface IEmailService
    {
        Task<SendResponse> SendEmail(RequestDetail requestDetail);
    }
}