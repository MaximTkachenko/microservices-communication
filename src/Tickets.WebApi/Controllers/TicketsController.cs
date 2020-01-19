using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Tickets.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ILogger<TicketsController> logger)
        {
            _logger = logger;
        }

        [HttpGet, Route("{userId}")]
        public IActionResult Get(long userId)
        {
            return Ok();
        }
    }
}
