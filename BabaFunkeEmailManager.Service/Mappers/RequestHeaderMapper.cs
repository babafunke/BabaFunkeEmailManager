using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace BabaFunkeEmailManager.Service.Mappers
{
    public class RequestHeaderMapper : IMapper<RequestHeader, RequestHeaderEntity>
    {
        public IEnumerable<RequestHeader> AllEntitiesToModels(IEnumerable<RequestHeaderEntity> entities)
        {
            return entities.Select(e => new RequestHeader
            {
                RequestHeaderId = e.RowKey,
                CreatedOn = e.CreatedOn,
                IsActive = e.IsActive,
                NewsletterId = e.NewsletterId,
                SubscriberSubCategory = e.PartitionKey
            }).ToList();
        }

        public RequestHeader EntityToModel(RequestHeaderEntity entity)
        {
            return new RequestHeader
            {
                RequestHeaderId = entity.RowKey,
                CreatedOn = entity.CreatedOn,
                IsActive = entity.IsActive,
                NewsletterId = entity.NewsletterId,
                SubscriberSubCategory = entity.PartitionKey
            };
        }

        public RequestHeaderEntity ModelToEntity(RequestHeader request)
        {
            return new RequestHeaderEntity
            {
                RowKey = request.RequestHeaderId,
                CreatedOn = request.CreatedOn,
                IsActive = request.IsActive,
                NewsletterId = request.NewsletterId,
                PartitionKey = request.SubscriberSubCategory
            };
        }
    }
}