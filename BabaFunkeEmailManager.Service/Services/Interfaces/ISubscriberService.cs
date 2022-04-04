using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Services.Interfaces
{
    /// <summary>
    /// An extension of the IService<T> interface for geting filtered list of Subscribers
    /// </summary>
    public interface ISubscriberService : IService<Subscriber>
    {
        Task<IEnumerable<Subscriber>> GetAllActiveSubscribersBySubCategory(string subCategory, string PartitionKey = "Subscriber");
    }
}