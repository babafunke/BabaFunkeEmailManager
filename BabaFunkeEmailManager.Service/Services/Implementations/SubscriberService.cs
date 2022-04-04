using BabaFunkeEmailManager.Data.Entities;
using BabaFunkeEmailManager.Data.Models;
using BabaFunkeEmailManager.Service.Mappers;
using BabaFunkeEmailManager.Service.Repositories.Interfaces;
using BabaFunkeEmailManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Service.Services.Implementations
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository _repository;
        private readonly IMapper<Subscriber, SubscriberEntity> _mapper;

        public SubscriberService(ISubscriberRepository repository, IMapper<Subscriber, SubscriberEntity> mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Subscriber>> GetAllItems()
        {
            var subscriberEntities = await _repository.GetAllEntities();

            var subscribers = _mapper.AllEntitiesToModels(subscriberEntities);

            return subscribers;
        }

        public async Task<IEnumerable<Subscriber>> GetAllActiveSubscribersBySubCategory(string subCategory, string PartitionKey = "Subscriber")
        {
            var subscriberEntities = await _repository.GetAllEntitiesBySubCategory(subCategory);

            var subscribers = _mapper.AllEntitiesToModels(subscriberEntities);

            return subscribers;
        }

        public async Task<ServiceResponse<Subscriber>> GetItem(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return ServiceResponse<Subscriber>.GetServiceResponse("Please provide the email!", null);
            }

            var subscriberEntity = await _repository.GetEntity(email.Replace(" ", "").ToLower(), "Subscriber");

            if (subscriberEntity == null)
            {
                return ServiceResponse<Subscriber>.GetServiceResponse($"Subscriber with Email {email} not found!", null);
            }

            var subscriber = _mapper.EntityToModel(subscriberEntity);

            return ServiceResponse<Subscriber>.GetServiceResponse("Subscriber success!", subscriber, true);
        }

        public async Task<ServiceResponse<Subscriber>> AddItem(Subscriber subscriber)
        {
            var existingSubscriber = await GetItem(subscriber.Email);

            if (existingSubscriber.IsSuccess)
            {
                return ServiceResponse<Subscriber>.GetServiceResponse($"Subscriber with email {subscriber.Email} already exists!", null);
            }

            subscriber.Id = Guid.NewGuid().ToString();
            subscriber.IsSubscribed = true;
            subscriber.SubscribedOn = DateTime.Now;

            var subscriberEntity = _mapper.ModelToEntity(subscriber);

            await _repository.AddEntity(subscriberEntity);

            return ServiceResponse<Subscriber>.GetServiceResponse("Subscriber added successfully!", null, true);
        }

        public async Task<ServiceResponse<Subscriber>> UpdateItem(string email, Subscriber subscriber)
        {
            var existingSubscriberResponse = await GetItem(email);

            if (!existingSubscriberResponse.IsSuccess)
            {
                return ServiceResponse<Subscriber>.GetServiceResponse($"Subscriber {subscriber.Email} not found!", null);
            }

            var existingSubscriber = existingSubscriberResponse.Data;

            if (!existingSubscriber.IsSubscribed)
            {
                return ServiceResponse<Subscriber>.GetServiceResponse($"{email} is not a subscriber!", null);
            }

            existingSubscriber.Firstname = subscriber.Firstname;
            existingSubscriber.Lastname = subscriber.Lastname;

            var subscriberEntity = _mapper.ModelToEntity(existingSubscriber);

            await _repository.UpdateEntity(subscriberEntity);

            return ServiceResponse<Subscriber>.GetServiceResponse("Subscriber updated successfully!", null, true);
        }

        public async Task<ServiceResponse<Subscriber>> DisableItem(string email)
        {
            var existingSubscriberResponse = await GetItem(email);

            if (!existingSubscriberResponse.IsSuccess)
            {
                return ServiceResponse<Subscriber>.GetServiceResponse($"Subscriber {email} not found!", null);
            }

            var subscriber = existingSubscriberResponse.Data;

            if (!subscriber.IsSubscribed)
            {
                return ServiceResponse<Subscriber>.GetServiceResponse($"Subscriber {email} has already been Unsubscribed!", null);
            }

            subscriber.IsSubscribed = false;
            subscriber.UnsubscribedOn = DateTime.Now;

            var subscriberEntity = _mapper.ModelToEntity(subscriber);

            await _repository.DiableEntity(subscriberEntity);

            return ServiceResponse<Subscriber>.GetServiceResponse($"Subscriber {email} successfully unsubscribed!", null, true);
        }

        public async Task<ServiceResponse<Subscriber>> DeleteItem(string email)
        {
            var existingSubscriberResponse = await GetItem(email);

            if (!existingSubscriberResponse.IsSuccess)
            {
                return ServiceResponse<Subscriber>.GetServiceResponse($"Subscriber {email} not found!", null);
            }

            var subscriber = existingSubscriberResponse.Data;

            var subscriberEntity = _mapper.ModelToEntity(subscriber);

            await _repository.DeleteEntity(subscriberEntity);

            return ServiceResponse<Subscriber>.GetServiceResponse($"Subscriber {email} successfully deleted!", null, true);
        }
    }
}