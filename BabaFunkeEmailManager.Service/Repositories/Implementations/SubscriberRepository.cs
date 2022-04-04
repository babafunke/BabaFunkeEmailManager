using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Service.Repositories.Interfaces;
using BabaFunkeEmailManager.Service.Utilities;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Repositories.Implementations
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly CloudTable _cloudTable;
        private readonly CloudTableClient _cloudTableClient;

        public SubscriberRepository(CloudTable cloudTable, CloudTableClient cloudTableClient)
        {
            _cloudTable = cloudTable;
            _cloudTableClient = cloudTableClient;
            _cloudTable = _cloudTableClient.GetTableReference(ServiceUtil.SubscriberTable);
        }

        public async Task<IEnumerable<SubscriberEntity>> GetAllEntities()
        {
            var query = new TableQuery<SubscriberEntity>();

            var entities = await _cloudTable.ExecuteQuerySegmentedAsync(query, null);

            return entities;
        }

        public async Task<IEnumerable<SubscriberEntity>> GetAllEntitiesBySubCategory(string subCategory)
        {
            var query1 = TableQuery.GenerateFilterCondition("SubCategory", QueryComparisons.Equal, subCategory);

            var query2 = TableQuery.GenerateFilterConditionForBool("IsSubscribed", QueryComparisons.Equal, true);

            var query = new TableQuery<SubscriberEntity>()
                .Where(TableQuery.CombineFilters(query1, TableOperators.And, query2));

            var entities = await _cloudTable.ExecuteQuerySegmentedAsync(query, null);

            return entities;
        }

        public async Task<SubscriberEntity> GetEntity(string rowKey, string partitionKey)
        {
            var operation = TableOperation.Retrieve<SubscriberEntity>(partitionKey, rowKey);

            var result = await _cloudTable.ExecuteAsync(operation);

            return result.Result as SubscriberEntity;
        }

        public async Task<bool> AddEntity(SubscriberEntity entity)
        {
            var operation = TableOperation.Insert(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> UpdateEntity(SubscriberEntity entity)
        {
            entity.ETag = "*";

            var operation = TableOperation.Replace(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> DiableEntity(SubscriberEntity entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> DeleteEntity(SubscriberEntity entity)
        {
            entity.ETag = "*";
            
            var operation = TableOperation.Delete(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }
    }
}