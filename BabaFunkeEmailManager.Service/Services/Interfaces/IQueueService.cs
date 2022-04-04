using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Services.Interfaces
{
    /// <summary>
    /// An interface for adding items to a Message Queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueueService<T>
    {
        Task AddItemToQueue(T item);
    }
}