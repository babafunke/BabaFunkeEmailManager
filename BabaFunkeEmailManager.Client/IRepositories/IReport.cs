using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.IRepositories
{
    public interface IReport
    {
        Task<IEnumerable<EmailResponse>> GetAllEmailResponse();
    }
}