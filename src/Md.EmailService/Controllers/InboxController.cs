using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Md.EmailService.Controllers
{
    [ApiController]
    public class InboxController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<InboxController> _logger;

        public InboxController(ILogger<InboxController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/inbox")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("/claims")]
        public IEnumerable<KeyValuePair<string, string>> Claims()
        {
            return User.Claims.Select(x => new KeyValuePair<string, string>(x.Type, x.Value));
        }
    }
}
