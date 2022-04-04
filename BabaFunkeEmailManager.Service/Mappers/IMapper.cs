using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace BabaFunkeEmailManager.Service.Mappers
{
    public interface IMapper<T, U> where U: TableEntity
    {
        IEnumerable<T> AllEntitiesToModels(IEnumerable<U> entities);
        T EntityToModel(U entity);
        U ModelToEntity(T item);
    }
}