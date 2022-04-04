using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.Repositories
{
    public class SubscriberService : ISubscriber
    {
        private readonly IHttpClientFactory _httpClient;
        public SubscriberService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<Subscriber>> GetAllSubscribers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "getallsubscribers");
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var subscribers = JsonConvert.DeserializeObject<IEnumerable<Subscriber>>(content);
            return subscribers;
        }

        public async Task<Subscriber> GetSubscriberByEmail(string email)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"getsubscriber/{email}");
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var subscriber = JsonConvert.DeserializeObject<Subscriber>(content);
            return subscriber;
        }

        public async Task<bool> CreateSubscriber(Subscriber subscriber)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"addsubscriber");
            var jsonBody = JsonConvert.SerializeObject(subscriber);
            var body = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
            request.Content = body;
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateSubscriber(string email, Subscriber subscriber)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"updatesubscriber/{email}");
            var jsonBody = JsonConvert.SerializeObject(subscriber);
            var body = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
            request.Content = body;
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Unsubscribe(string email)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, $"unsubscribe");
            var subscriber = new Subscriber { Email = email };
            var jsonBody = JsonConvert.SerializeObject(subscriber);
            var body = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
            request.Content = body;
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteSubscriber(string email)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"deletesubscriber/{email}");
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}
