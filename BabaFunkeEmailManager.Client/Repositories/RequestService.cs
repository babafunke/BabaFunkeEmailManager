using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.Repositories
{
    /// <summary>
    /// Implements the IRequest CRUD operations by consuming the API endpoints defined
    /// in the BabaFunkeEmailManager.Api project using the HttpClient
    /// </summary>
    public class RequestService : IRequest
    {
        private readonly IHttpClientFactory _httpClient;

        public RequestService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<RequestHeader>> GetAllRequests()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "getallrequestheaders");
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var requests = JsonConvert.DeserializeObject<IEnumerable<RequestHeader>>(content);
            return requests;
        }

        public async Task<bool> CreateRequest(RequestHeader req)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"addrequest");
            var jsonBody = JsonConvert.SerializeObject(req);
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

        public async Task<bool> DeleteRequest(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"deleterequestheader/{id}");
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