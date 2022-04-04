using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Services.Interfaces
{
    /// <summary>
    /// Generic interface for basic CRUD operations
    /// </summary>
    /// <typeparam name="T">The Model - Subscriber, Request or Newsletter</typeparam>
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAllItems();
        Task<ServiceResponse<T>> GetItem(string id);
        Task<ServiceResponse<T>> AddItem(T item);
        Task<ServiceResponse<T>> UpdateItem(string id, T item);
        Task<ServiceResponse<T>> DisableItem(string id);
        Task<ServiceResponse<T>> DeleteItem(string id);
    }
}