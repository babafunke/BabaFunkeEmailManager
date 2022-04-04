using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.IRepositories
{
    public interface INewsletter
    {
        Task<IEnumerable<Newsletter>> GetAllNewsletters();
        Task<Newsletter> GetNewsletterById(string id);
        Task<bool> CreateNewsletter(Newsletter newsletter);
        Task<bool> EditNewsletter(Newsletter newsletter);
        Task<bool> DeleteNewsletter(string id);
    }
}