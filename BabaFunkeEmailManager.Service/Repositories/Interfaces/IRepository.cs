using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Repositories.Interfaces
{
    /// <summary>
    /// A generic interface for CRUD operations to and from Azure Table Storage
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> where T: TableEntity
    {
        Task<IEnumerable<T>> GetAllEntities();
        Task<T> GetEntity(string rowKey, string partitionKey);
        Task<bool> AddEntity(T entity);
        Task<bool> UpdateEntity(T entity);
        Task<bool> DiableEntity(T entity);
        Task<bool> DeleteEntity(T entity);
    }
}