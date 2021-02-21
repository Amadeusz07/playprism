using Microsoft.AspNetCore.Mvc.Filters;

namespace Playprism.Services.TeamService.API.Filters
{
    public class UserAuth0Filter : IActionFilter
    {

        public UserAuth0Filter()
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
