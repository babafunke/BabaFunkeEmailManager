using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Service.Repositories.Interfaces;
using BabaFunkeEmailManager.Service.Utilities;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Repositories.Implementations
{
    public class RequestHeaderRepository : IRepository<RequestHeaderEntity>
    {
        private readonly CloudTable _cloudTable;
        private readonly CloudTableClient _cloudTableClient;

        public RequestHeaderRepository(CloudTable cloudTable, CloudTableClient cloudTableClient)
        {
            _cloudTable = cloudTable;
            _cloudTableClient = cloudTableClient;
            _cloudTable = _cloudTableClient.GetTableReference(ServiceUtil.RequestTable);
        }

        public async Task<IEnumerable<RequestHeaderEntity>> GetAllEntities()
        {
            var query = new TableQuery<RequestHeaderEntity>();

            var entities = await _cloudTable.ExecuteQuerySegmentedAsync(query, null);

            return entities;
        }

        public async Task<RequestHeaderEntity> GetEntity(string rowKey, string partitionKey)
        {
            var operation = TableOperation.Retrieve<RequestHeaderEntity>(partitionKey, rowKey);

            var result = await _cloudTable.ExecuteAsync(operation);

            return result.Result as RequestHeaderEntity;
        }

        public async Task<bool> AddEntity(RequestHeaderEntity entity)
        {
            entity.PartitionKey = "Request";

            var operation = TableOperation.Insert(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> UpdateEntity(RequestHeaderEntity entity)
        {
            entity.ETag = "*";

            var operation = TableOperation.Replace(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> DiableEntity(RequestHeaderEntity entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> DeleteEntity(RequestHeaderEntity entity)
        {
            entity.ETag = "*";

            var operation = TableOperation.Delete(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }
    }
}