using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Mappers;
using BabaFunkeEmailManager.Service.Repositories.Interfaces;
using BabaFunkeEmailManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Services.Implementations
{
    public class RequestHeaderService : IService<RequestHeader>
    {
        private readonly IRepository<RequestHeaderEntity> _repository;
        private readonly IMapper<RequestHeader, RequestHeaderEntity> _mapper;

        public RequestHeaderService(IRepository<RequestHeaderEntity> repository, IMapper<RequestHeader, RequestHeaderEntity> mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RequestHeader>> GetAllItems()
        {
            var requestHeaderEntities = await _repository.GetAllEntities();

            var requestHeaders = _mapper.AllEntitiesToModels(requestHeaderEntities);

            return requestHeaders;
        }

        public async Task<ServiceResponse<RequestHeader>> GetItem(string requestHeaderId)
        {
            if (string.IsNullOrEmpty(requestHeaderId))
            {
                return ServiceResponse<RequestHeader>.GetServiceResponse("RequestHeader Id cannot be null or empty!", null);
            }
            var requestHeaderEntity = await _repository.GetEntity(requestHeaderId, "Request");

            if (requestHeaderEntity == null)
            {
                return ServiceResponse<RequestHeader>.GetServiceResponse($"RequestHeader with Id {requestHeaderId} not found!", null);
            }

            var requestHeader = _mapper.EntityToModel(requestHeaderEntity);

            return ServiceResponse<RequestHeader>.GetServiceResponse("RequestHeader success!", requestHeader, true);
        }

        public async Task<ServiceResponse<RequestHeader>> AddItem(RequestHeader requestHeader)
        {
            var existingRequests = await GetAllItems();

            if(existingRequests.Any())
            {
                var maxId = existingRequests.Max(r => int.Parse(r.RequestHeaderId));

                requestHeader.RequestHeaderId = (maxId + 1).ToString();
            }
            else
            {
                requestHeader.RequestHeaderId = "1";
            }

            var requestHeaderEntity = _mapper.ModelToEntity(requestHeader);

            await _repository.AddEntity(requestHeaderEntity);

            return ServiceResponse<RequestHeader>.GetServiceResponse("RequestHeader added successfully!", null, true);
        }

        public async Task<ServiceResponse<RequestHeader>> UpdateItem(string requestHeaderId, RequestHeader requestHeader)
        {
            var existingRequestHeaderResponse = await GetItem(requestHeaderId);

            if (!existingRequestHeaderResponse.IsSuccess)
            {
                return existingRequestHeaderResponse;
            }

            var existingRequestHeader = existingRequestHeaderResponse.Data;

            existingRequestHeader.NewsletterId = requestHeader.NewsletterId;
            existingRequestHeader.SubscriberSubCategory = requestHeader.SubscriberSubCategory;

            var requestHeaderEntity = _mapper.ModelToEntity(existingRequestHeader);

            await _repository.UpdateEntity(requestHeaderEntity);

            return ServiceResponse<RequestHeader>.GetServiceResponse("RequestHeader updated successfully!", null, true);
        }

        public async Task<ServiceResponse<RequestHeader>> DisableItem(string requestHeaderId)
        {
            var existingRequestHeaderResponse = await GetItem(requestHeaderId);

            if (!existingRequestHeaderResponse.IsSuccess)
            {
                return existingRequestHeaderResponse;
            }

            var existingRequestHeader = existingRequestHeaderResponse.Data;

            existingRequestHeader.IsActive = false;

            var requestHeaderEntity = _mapper.ModelToEntity(existingRequestHeader);

            await _repository.DiableEntity(requestHeaderEntity);

            return ServiceResponse<RequestHeader>.GetServiceResponse($"RequestHeader {requestHeaderId} successfully disabled!", null, true);
        }

        public async Task<ServiceResponse<RequestHeader>> DeleteItem(string requestHeaderId)
        {
            var existingRequestHeaderResponse = await GetItem(requestHeaderId);

            if (!existingRequestHeaderResponse.IsSuccess)
            {
                return existingRequestHeaderResponse;
            }

            var existingRequestHeader = existingRequestHeaderResponse.Data;

            var requestHeaderEntity = _mapper.ModelToEntity(existingRequestHeader);

            await _repository.DeleteEntity(requestHeaderEntity);

            return ServiceResponse<RequestHeader>.GetServiceResponse($"RequestHeader {requestHeaderId} successfully deleted!", null, true);
        }

    }
}