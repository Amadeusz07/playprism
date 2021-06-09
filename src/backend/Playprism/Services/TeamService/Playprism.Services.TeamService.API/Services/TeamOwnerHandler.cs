using Microsoft.AspNetCore.Authorization;
using Playprism.Services.TeamService.API.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Services
{
    public class TeamOwnerHandler : AuthorizationHandler<TeamOwnerRequirement, TeamEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamOwnerRequirement requirement, TeamEntity resource)
        {
            var id = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (id == resource.OwnerId.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class TeamOwnerRequirement : IAuthorizationRequirement { }
}
