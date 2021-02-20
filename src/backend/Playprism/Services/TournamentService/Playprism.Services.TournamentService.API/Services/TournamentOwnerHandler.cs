using Microsoft.AspNetCore.Authorization;
using Playprism.Services.TournamentService.DAL.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.API.Services
{
    public class TournamentOwnerHandler: AuthorizationHandler<TournamentOwnerRequirement, TournamentEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TournamentOwnerRequirement requirement, TournamentEntity resource)
        {
            var id = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (id == resource.OwnerId.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class TournamentOwnerRequirement: IAuthorizationRequirement { }
}
