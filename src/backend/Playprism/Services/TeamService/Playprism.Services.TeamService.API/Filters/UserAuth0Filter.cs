using Microsoft.AspNetCore.Mvc.Filters;
using Playprism.Services.TeamService.API.Repositories;
using System;
using System.Linq;
using System.Security.Claims;

namespace Playprism.Services.TeamService.API.Filters
{
    public class UserAuth0Filter : IActionFilter
    {
        private readonly TeamDbContext _teamDbContext;

        public UserAuth0Filter(TeamDbContext teamDbContext)
        {
            _teamDbContext = teamDbContext;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = context.HttpContext.User.FindFirstValue(ClaimTypes.Name); 
            var ex = _teamDbContext.Players.Where(x => x.UserId == 0);
            if (userId != null && userName != null)
            {
                Console.WriteLine("smack that");

            }
        }
    }
}
