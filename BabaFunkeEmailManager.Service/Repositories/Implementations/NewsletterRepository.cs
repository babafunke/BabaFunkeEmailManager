using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Service.Repositories.Interfaces;
using BabaFunkeEmailManager.Service.Utilities;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BabaFunkeEmailManager.Service.Repositories.Implementations
{
    public class NewsletterRepository : IRepository<NewsletterEntity>
    {
        private readonly CloudTable _cloudTable;
        private readonly CloudTableClient _cloudTableClient;

        public NewsletterRepository(CloudTable cloudTable, CloudTableClient cloudTableClient)
        {
            _cloudTable = cloudTable;
            _cloudTableClient = cloudTableClient;
            _cloudTable = _cloudTableClient.GetTableReference(ServiceUtil.NewsletterTable);
        }

        public async Task<IEnumerable<NewsletterEntity>> GetAllEntities()
        {
            var query = new TableQuery<NewsletterEntity>();

            var entities = await _cloudTable.ExecuteQuerySegmentedAsync(query, null);

            return entities;
        }

        public async Task<NewsletterEntity> GetEntity(string rowKey, string partitionKey)
        {
            var operation = TableOperation.Retrieve<NewsletterEntity>(partitionKey, rowKey);

            var result = await _cloudTable.ExecuteAsync(operation);

            return result.Result as NewsletterEntity;
        }

        public async Task<bool> AddEntity(NewsletterEntity entity)
        {
            var operation = TableOperation.Insert(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> UpdateEntity(NewsletterEntity entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> DiableEntity(NewsletterEntity entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);

            await _cloudTable.ExecuteAsync(operation);

            return true;
        }

        public async Task<bool> DeleteEntity(NewsletterEntity entity)
        {
            entity.ETag = "*";

            var operation = TableOperation.Delete(entity);

            var result = await _cloudTable.ExecuteAsync(operation);

            return true;
        }
    }
}