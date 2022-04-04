using BabaFunkeEmailManager.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Repositories.Interfaces
{
    public interface ISubscriberRepository : IRepository<SubscriberEntity>
    {
        Task<IEnumerable<SubscriberEntity>> GetAllEntitiesBySubCategory(string subCategory);
    }
}