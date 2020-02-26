using System;
using System.Threading.Tasks;
using Users.WebApi.Db;

namespace Users.WebApi.Services
{
    public class ClaimsService : IClaimsService
    {
        public readonly UsersDb _db;

        public ClaimsService(UsersDb db)
        {
            _db = db;
        }

        public Task<(string Type, string Value, string ValueType, string Issuer)[]> GetClaimsAsync(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
