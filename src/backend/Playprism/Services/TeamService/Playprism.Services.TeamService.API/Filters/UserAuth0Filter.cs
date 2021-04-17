using Microsoft.AspNetCore.Mvc.Filters;
using Playprism.Services.TeamService.API.Interface.Repositories;
using Playprism.Services.TeamService.API.Interfaces.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Filters
{
    public class UserAuth0Filter : IAsyncActionFilter
    {
        private readonly IPlayerService _playerService;
        private readonly IAuth0Repository _auth0Repository;

        public UserAuth0Filter(IPlayerService playerService, IAuth0Repository auth0Repository)
        {
            _playerService = playerService;
            _auth0Repository = auth0Repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var player = await _playerService.GetPlayerByUserIdAsync(userId);
            if (player == null)
            {
                var userInfo = await _auth0Repository.GetUserByUserId(userId);
                await _playerService.HandleUnknownUserAsync(userInfo);
            }

            await next();
        }
    }
}