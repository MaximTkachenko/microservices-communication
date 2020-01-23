using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace Tickets.Daemon
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _http;
        private readonly AuthenticationContext _authContext;
        private readonly ClientCredential _credential;
        private readonly string _usersApiBaseUrl;
        private readonly AzureAdOptions _azureAdConfig;

        public Worker(ILogger<Worker> logger,
            IHttpClientFactory http,
            TokenCache tokenCache,
            IConfiguration config,
            IOptions<AzureAdOptions> azureAdConfig)
        {
            _logger = logger;
            _http = http;
            _authContext = new AuthenticationContext(_azureAdConfig.Authority, tokenCache);
            _azureAdConfig = azureAdConfig.Value;
            _credential = new ClientCredential(_azureAdConfig.ClientId, _azureAdConfig.ClientSecret);
            _usersApiBaseUrl = config.GetValue<string>("Services:UsersApiUrl");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var tokenResult = await _authContext.AcquireTokenAsync(_azureAdConfig.Resource, _credential);

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
