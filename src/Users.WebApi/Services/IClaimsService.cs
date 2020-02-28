using System.Threading.Tasks;
using Common.UsersApiModels;

namespace Users.WebApi.Services
{
    public interface IClaimsService
    {
        Task<TheAppClaim[]> GetClaimsAsync(long userId);
    }
}
