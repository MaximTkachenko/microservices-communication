using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.WebApi.Db;

namespace Users.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly UsersDb _db;

        public UsersController(IAuthorizationService authorizationService,
            UsersDb db)
        {
            _authorizationService = authorizationService;
            _db = db;
        }

        [HttpGet, Route("{userIdOrEmail}/claims")]
        public async Task<IActionResult> GetUserClaims(string userIdOrEmail)
        {
            var user = long.TryParse(userIdOrEmail, out var userId)
                ? await _db.Users.FirstAsync(x => x.Id == userId)
                : await _db.Users.FirstAsync(x => x.Email == userIdOrEmail);

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, user, Operations.Read);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var theAppClaims = await _db.Claims.Where(x => x.UserId == user.Id).ToArrayAsync();
            return Ok(theAppClaims);
        }

        [HttpDelete, Route("{userId}/claims/{claimId}")]
        //todo check permissions: daemon, owner or admin
        public IActionResult DeleteClaim(long userId, long claimId)
        {
            return null;
        }

        [HttpGet, Route("{userId}")]
        //todo check permissions: daemon, owner or admin
        public IActionResult GetUser(long userId)
        {
            return null;
        }

        [HttpPost, Route("")]
        //todo check permissions: daemon, owner or admin
        public IActionResult CreateUser()
        {
            return null;
        }

        [HttpPut, Route("{userId}")]
        //todo check permissions: daemon, owner or admin
        public IActionResult UpdateUser(long userId)
        {
            return null;
        }

        [HttpGet, Route("")]
        public IActionResult GetUsers()
        {
            return null;
        }
    }
}