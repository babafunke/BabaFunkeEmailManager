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
    public class NewsletterService : IService<Newsletter>
    {
        private readonly IRepository<NewsletterEntity> _repository;
        private readonly IMapper<Newsletter, NewsletterEntity> _mapper;

        public NewsletterService(IRepository<NewsletterEntity> repository, IMapper<Newsletter, NewsletterEntity> mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Newsletter>> GetAllItems()
        {
            var newsletterEntities = await _repository.GetAllEntities();

            var newsletters = _mapper.AllEntitiesToModels(newsletterEntities);

            return newsletters;
        }

        public async Task<ServiceResponse<Newsletter>> GetItem(string newsletterId)
        {
            if (string.IsNullOrEmpty(newsletterId))
            {
                return ServiceResponse<Newsletter>.GetServiceResponse("Newsletter Id cannot be null or empty!", null);
            }
            var newsletterEntity = await _repository.GetEntity(newsletterId, "Newsletter");

            if (newsletterEntity == null)
            {
                return ServiceResponse<Newsletter>.GetServiceResponse($"Newsletter with Id {newsletterId} not found!", null);
            }

            var newsletter = _mapper.EntityToModel(newsletterEntity);

            return ServiceResponse<Newsletter>.GetServiceResponse("Newsletter success!", newsletter, true);
        }

        public async Task<ServiceResponse<Newsletter>> AddItem(Newsletter newsletter)
        {
            var existingNewsletter = await GetItem(newsletter.NewsletterId);

            if (existingNewsletter.IsSuccess)
            {
                return ServiceResponse<Newsletter>.GetServiceResponse($"Newsletter with Id {newsletter.NewsletterId} already exists!", null);
            }

            var newsletterEntity = _mapper.ModelToEntity(newsletter);

            await _repository.AddEntity(newsletterEntity);

            return ServiceResponse<Newsletter>.GetServiceResponse("Newsletter added successfully!", null, true);
        }

        public async Task<ServiceResponse<Newsletter>> UpdateItem(string newsletterId, Newsletter newsletter)
        {
            var existingNewsletterResponse = await GetItem(newsletterId);

            if (!existingNewsletterResponse.IsSuccess)
            {
                return existingNewsletterResponse;
            }

            var existingNewsletter = existingNewsletterResponse.Data;

            existingNewsletter.Subject = newsletter.Subject;
            existingNewsletter.Body = newsletter.Body;

            var newsletterEntity = _mapper.ModelToEntity(existingNewsletter);

            await _repository.UpdateEntity(newsletterEntity);

            return ServiceResponse<Newsletter>.GetServiceResponse("Newsletter updated successfully!", null, true);
        }

        public async Task<ServiceResponse<Newsletter>> DisableItem(string newsletterId)
        {
            var existingNewsletterResponse = await GetItem(newsletterId);

            if (!existingNewsletterResponse.IsSuccess)
            {
                return existingNewsletterResponse;
            }

            var existingNewsletter = existingNewsletterResponse.Data;

            existingNewsletter.IsEnabled = false;

            var newsletterEntity = _mapper.ModelToEntity(existingNewsletter);

            await _repository.DiableEntity(newsletterEntity);

            return ServiceResponse<Newsletter>.GetServiceResponse($"Newsletter {newsletterId} successfully disabled!", null, true);
        }

        public async Task<ServiceResponse<Newsletter>> DeleteItem(string newsletterId)
        {
            var existingNewsletterResponse = await GetItem(newsletterId);

            if (!existingNewsletterResponse.IsSuccess)
            {
                return existingNewsletterResponse;
            }

            var existingNewsletter = existingNewsletterResponse.Data;

            var newsletterEntity = _mapper.ModelToEntity(existingNewsletter);

            await _repository.DeleteEntity(newsletterEntity);

            return ServiceResponse<Newsletter>.GetServiceResponse($"Newsletter {newsletterId} successfully deleted!", null, true);
        }

    }
}