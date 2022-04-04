using BabaFunkeEmailManager.Client.IRepositories;
using BabaFunkeEmailManager.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BabaFunkeEmailManager.Client.Repositories
{
    public class ReportService : IReport
    {
        private readonly IHttpClientFactory _httpClient;

        public ReportService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<EmailResponse>> GetAllEmailResponse()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "getallreports");
            var client = _httpClient.CreateClient("httpclient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var reports = JsonConvert.DeserializeObject<IEnumerable<EmailResponse>>(content);
            return reports;
        }
    }
}