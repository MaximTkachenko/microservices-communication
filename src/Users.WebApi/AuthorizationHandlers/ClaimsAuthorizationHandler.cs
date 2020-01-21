using System.Linq;
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
            User user)
        {
            //it's a daemon application, all operations are allowed
            if (context.User.Identities.First().Claims.Any(x =>
                x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && x.Value == "Daemon"))
            {
                context.Succeed(requirement);
            }

            //user is an admin of customer
            if (context.User.Identities.First().Claims.Any(x =>
                x.Type == "x-customer-admin" && x.Value == user.CustomerId.ToString()))
            {
                context.Succeed(requirement);
            }

            //user wants to get own claims
            if(context.User.Identities.First().Claims.Any(x =>
                x.Type == "x-userId" && x.Value == user.Id.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
