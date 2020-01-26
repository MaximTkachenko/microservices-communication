using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Users.WebApi.Db;

namespace Users.WebApi.AuthorizationHandlers
{
    public class UserAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement,
            User user)
        {
            //it's a daemon application, all operations are allowed
            if (context.User.TryFindClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Daemon"))
            {
                context.Succeed(requirement);
            }

            //user is an admin of customer
            if (context.User.TryFindClaim("x-customer-admin", user.CustomerId.ToString()))
            {
                context.Succeed(requirement);
            }

            //user wants to get own claims
            if(context.User.TryFindClaim("x-userId", user.Id.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
