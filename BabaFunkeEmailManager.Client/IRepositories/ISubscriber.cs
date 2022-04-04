using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.IRepositories
{
    public interface ISubscriber
    {
        Task<IEnumerable<Subscriber>> GetAllSubscribers();
        Task<Subscriber> GetSubscriberByEmail(string email);
        Task<bool> CreateSubscriber(Subscriber subscriber);
        Task<bool> UpdateSubscriber(string email, Subscriber subscriber);
        Task<bool> DeleteSubscriber(string email);
        Task<bool> Unsubscribe(string email);
    }
}
