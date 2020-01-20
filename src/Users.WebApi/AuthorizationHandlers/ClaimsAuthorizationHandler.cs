using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Users.WebApi.Db;

namespace Users.WebApi.AuthorizationHandlers
{
    public class ClaimsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement,
            User resource)
        {
            //"http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            return Task.CompletedTask;
        }
    }
}
