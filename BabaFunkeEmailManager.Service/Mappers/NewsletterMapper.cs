using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace BabaFunkeEmailManager.Service.Mappers
{
    public class NewsletterMapper : IMapper<Newsletter, NewsletterEntity>
    {
        public IEnumerable<Newsletter> AllEntitiesToModels(IEnumerable<NewsletterEntity> entities)
        {
            return entities.Select(e => new Newsletter
            {
                NewsletterId = e.RowKey,
                Category = e.PartitionKey,
                Subject = e.Subject,
                Body = e.Body,
                CreatedOn = e.CreatedOn,
                IsEnabled = e.IsEnabled
            }).ToList();
        }

        public Newsletter EntityToModel(NewsletterEntity entity)
        {
            return new Newsletter
            {
                NewsletterId = entity.RowKey,
                Category = entity.PartitionKey,
                Subject = entity.Subject,
                Body = entity.Body,
                CreatedOn = entity.CreatedOn,
                IsEnabled = entity.IsEnabled
            };
        }

        public NewsletterEntity ModelToEntity(Newsletter newsletter)
        {
            return new NewsletterEntity
            {
                RowKey = newsletter.NewsletterId,
                PartitionKey = newsletter.Category,
                Subject = newsletter.Subject,
                Body = newsletter.Body,
                CreatedOn = newsletter.CreatedOn,
                IsEnabled = newsletter.IsEnabled
            };
        }
    }
}