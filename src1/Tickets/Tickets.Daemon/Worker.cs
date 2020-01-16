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

namespace Tickets.Daemon
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
            _credential = new ClientCredential("da51a2ec-058f-4025-a75a-41af428be001", "9T/R7A2c?AZ4GAhkNWP]L=0UyH2ndXB6");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var tokenResult = await _authContext.AcquireTokenAsync("api://theapp.api", _credential);
                //"roles": [ "Daemon" ]
                Console.WriteLine(tokenResult.AccessToken);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
