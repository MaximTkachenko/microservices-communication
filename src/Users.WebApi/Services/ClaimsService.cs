using System;
using System.Threading.Tasks;
using Common.UsersApiModels;
using Microsoft.EntityFrameworkCore;
using Users.WebApi.Db;

namespace Users.WebApi.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly UsersDb _db;

        public ClaimsService(UsersDb db)
        {
            _db = db;
        }

        public async Task<TheAppClaim[]> GetClaimsAsync(long userId)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return Array.Empty<TheAppClaim>();
            }

            return new []
            {
                new TheAppClaim{ClaimType = TheAppClaim.UserId, ClaimValue = user.Id.ToString(), ClaimValueType = "integer"},
                new TheAppClaim{ClaimType = TheAppClaim.CustomerId, ClaimValue = user.CustomerId.ToString(), ClaimValueType = "integer"}
            };
        }
    }
}
