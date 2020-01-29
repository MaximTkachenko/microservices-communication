using System.Threading.Tasks;

namespace Users.WebApi.Services
{
    public interface IClaimsService
    {
        Task<(string Type, string Value, string ValueType, string Issuer)[]> GetClaimsAsync(long userId);
    }
}
