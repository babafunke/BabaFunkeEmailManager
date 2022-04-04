using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace BabaFunkeEmailManager.Service.Mappers
{
    public class SubscriberMapper : IMapper<Subscriber, SubscriberEntity>
    {
        public IEnumerable<Subscriber> AllEntitiesToModels(IEnumerable<SubscriberEntity> entities)
        {
            return entities.Select(e => new Subscriber
            {
                Id = e.Id,
                Firstname = e.Firstname,
                Lastname = e.Lastname,
                Email = e.RowKey,
                Category = e.PartitionKey,
                SubCategory = e.SubCategory,
                IsSubscribed = e.IsSubscribed,
                SubscribedOn = e.SubscribedOn,
                UnsubscribedOn = e.UnsubscribedOn
            }).ToList();
        }

        public Subscriber EntityToModel(SubscriberEntity entity)
        {
            return new Subscriber
            {
                Id = entity.Id,
                Firstname = entity.Firstname,
                Lastname = entity.Lastname,
                Email = entity.RowKey,
                Category = entity.PartitionKey,
                SubCategory = entity.SubCategory,
                IsSubscribed = entity.IsSubscribed,
                SubscribedOn = entity.SubscribedOn,
                UnsubscribedOn = entity.UnsubscribedOn
            };
        }

        public SubscriberEntity ModelToEntity(Subscriber subscriber)
        {
            return new SubscriberEntity
            {
                RowKey = subscriber.Email.Replace(" ", "").ToLower(),
                PartitionKey = subscriber.Category,
                Id = subscriber.Id,
                Firstname = subscriber.Firstname,
                Lastname = subscriber.Lastname,
                SubCategory= subscriber.SubCategory,
                IsSubscribed = subscriber.IsSubscribed,
                SubscribedOn = subscriber.SubscribedOn,
                UnsubscribedOn = subscriber.UnsubscribedOn
            };
        }
    }
}