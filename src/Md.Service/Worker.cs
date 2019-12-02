using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace Md.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthenticationContext _authContext;
        private readonly ClientCredential _credential;

        public Worker(ILogger<Worker> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _authContext = new AuthenticationContext("https://login.microsoftonline.com/6b9be1b6-4f80-4ce7-8479-16c4d7726470");
            _credential = new ClientCredential("3d572aaa-e4c1-4d35-975a-c3b8dd3da053", "f0fl?XeBcHmLffPHI6p5T7oRqnv_ja[?");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var tokenResult = await _authContext.AcquireTokenAsync("api://md.backend", _credential);
                Console.WriteLine(tokenResult.AccessToken);

                var client = _httpClientFactory.CreateClient();

                var inboxRequest = new HttpRequestMessage(HttpMethod.Get, " https://localhost:44301/api/email/inbox");
                inboxRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.AccessToken);
                var inboxResponse = await client.SendAsync(inboxRequest);
                _logger.LogInformation(await inboxResponse.Content.ReadAsStringAsync());

                var claimsRequest = new HttpRequestMessage(HttpMethod.Get, " https://localhost:44301/api/email/claims");
                claimsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.AccessToken);
                var claimsResponse = await client.SendAsync(claimsRequest);
                _logger.LogInformation(await claimsResponse.Content.ReadAsStringAsync());

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
