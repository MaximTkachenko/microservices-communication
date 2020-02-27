using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Tickets.Daemon
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _http;
        private readonly string _usersApiBaseUrl;
        private readonly IConfidentialClientApplication _clientApplication;

        public Worker(ILogger<Worker> logger,
            IHttpClientFactory http,
            IConfiguration config,
            IOptions<AzureAdOptions> azureAdConfig)
        {
            _logger = logger;
            _http = http;
            _usersApiBaseUrl = config.GetValue<string>("Services:UsersApiUrl");

            var co = new ConfidentialClientApplicationOptions
            {
                Instance = "https://login.microsoftonline.com/",
                TenantId = "6b9be1b6-4f80-4ce7-8479-16c4d7726470",
                ClientId = azureAdConfig.Value.ClientId,
                ClientSecret = azureAdConfig.Value.ClientSecret
            };

            _clientApplication = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(co)
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTime.Now);

                var tokenResult = await _clientApplication.AcquireTokenForClient(new []{ "api://theapp.api/.default" })
                    .ExecuteAsync(stoppingToken);

                Console.WriteLine(tokenResult.AccessToken);

                //reading claims for any user
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_usersApiBaseUrl}/api/users/oblomov86@gmail.com/claims");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.AccessToken);
                var response = await _http.CreateClient().SendAsync(request, stoppingToken);
                var claims = await response.Content.ReadAsStringAsync();
                Console.WriteLine(claims);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
