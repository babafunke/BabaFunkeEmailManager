using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.Repositories
{
    public class NewsletterService : INewsletter
    {
        private readonly IHttpClientFactory _httpClient;
        public NewsletterService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<Newsletter>> GetAllNewsletters()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "getallnewsletters");
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var newsletters = JsonConvert.DeserializeObject<IEnumerable<Newsletter>>(content);
            return newsletters;
        }

        public async Task<Newsletter> GetNewsletterById(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"getnewsletter/{id}");
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var newsletter = JsonConvert.DeserializeObject<Newsletter>(content);
            return newsletter;
        }

        public async Task<bool> CreateNewsletter(Newsletter newsletter)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"addnewsletter");
            var jsonBody = JsonConvert.SerializeObject(newsletter);
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

        public async Task<bool> EditNewsletter(Newsletter newsletter)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"updatenewsletter/{newsletter.NewsletterId}");
            var jsonBody = JsonConvert.SerializeObject(newsletter);
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

        public async Task<bool> DeleteNewsletter(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"deletenewsletter/{id}");
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
