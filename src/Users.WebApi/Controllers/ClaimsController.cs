using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Users.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        [HttpGet, Route("{userIdOrEmail}")]
        //todo check permissions: daemon, owner or admin
        public IActionResult GetClaims(string userIdOrEmail)
        {
            if (!long.TryParse(userIdOrEmail, out var userId))
            {
                //todo get userId by email
            }

            //todo it's better to read from db because for daemon this logic doesn't work
            var theAppClaims = User.Identities.First().Claims.Where(x => x.Issuer == "theapp")
                .Select(x => new KeyValuePair<string, string>(x.Type, x.Value));
            return Ok(theAppClaims);
        }
    }
}