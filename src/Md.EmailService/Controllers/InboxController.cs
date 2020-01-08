using System.Collections.Generic;
using System.Linq;
using Md.EmailService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Md.EmailService.Controllers
{
    [ApiController]
    public class InboxController : ControllerBase
    {
        private readonly ILogger<InboxController> _logger;

        public InboxController(ILogger<InboxController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/inbox")]
        public ActionResult<IEnumerable<InboxEmailItem>> Get()
        {
            return Ok(Enumerable.Range(1, 5).Select(index => new InboxEmailItem()).ToArray());
        }

        [HttpGet]
        [Route("/claims")]
        public ActionResult<IEnumerable<KeyValuePair<string, string>>> Claims()
        {
            return Ok(User.Claims.Select(x => new KeyValuePair<string, string>(x.Type, x.Value)));
        }
    }
}
