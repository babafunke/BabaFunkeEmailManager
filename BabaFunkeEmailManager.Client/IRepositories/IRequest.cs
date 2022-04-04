using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.IRepositories
{
    /// <summary>
    /// The interface for the Request CRUD operations
    /// </summary>
    public interface IRequest
    {
        Task<IEnumerable<RequestHeader>> GetAllRequests();
        Task<bool> CreateRequest(RequestHeader request);
        Task<bool> DeleteRequest(string id);
    }
}