using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playprism.Services.TeamService.API.Dtos;
using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamManageService _teamManageService;
        private readonly IAuthorizationService _authorizationService;

        public TeamController(ITeamManageService teamManageService, IAuthorizationService authorizationService)
        {
            _teamManageService = teamManageService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AssignmentResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TeamEntity>>> Get()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var entities = await _teamManageService.GetAssignments(userId);

            return Ok(entities);
        }

        [HttpGet("current-team")]
        [ProducesResponseType(typeof(TeamEntity), StatusCodes.Status200OK)]
        public async Task<ActionResult<TeamEntity>> GetCurrentTeam()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _teamManageService.GetCurrentTeamAsync(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TeamEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeamEntity>> GetById(int id)
        {
            var entity = await _teamManageService.GetTeamAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TeamEntity), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<TeamEntity>> Post([FromBody] TeamEntity entity)
        {
            entity.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                entity = await _teamManageService.AddTeamAsync(entity);
            }
            catch (ValidationException ex)
            {
                return Conflict(ex.Message);
            }

            return Accepted(entity);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] TeamEntity entity)
        {
            var team = await _teamManageService.GetTeamAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, team, "TeamOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            if (team == null)
            {
                return NotFound();
            }
            try
            {
                await _teamManageService.UpdateTeamAsync(id, entity);
            }
            catch (ValidationException ex)
            {
                return Conflict(ex.Message);
            }


            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var team = await _teamManageService.GetTeamAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, team, "TeamOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            if (team == null)
            {
                return NotFound();
            }

            await _teamManageService.DeleteTeamAsync(team);

            return Accepted();
        }

        [HttpPost("{id}/invite/{username}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> InvitePlayerToTeam(int id, string username)
        {
            var team = await _teamManageService.GetTeamAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, team, "TeamOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            if (team == null)
            {
                return NotFound();
            }
            if (username == "")
            {
                return BadRequest();
            }
            try
            {
                await _teamManageService.InvitePlayerAsync(id, username);
            }
            catch (ValidationException ex)
            {
                return Conflict(ex.Message);
            }

            return Accepted();
        }

        [HttpPost("{id}/join")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<TeamEntity>> JoinTeam(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                await _teamManageService.JoinTeamAsync(userId, id);
            }
            catch (ValidationException ex)
            {
                return Conflict(ex.Message);
            }

            return Accepted();
        }

        [HttpPost("{id}/refuse")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> RefuseTeam(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _teamManageService.RefuseTeamAsync(userId, id);

            return Accepted();
        }

        [HttpPost("{id}/leave")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<TeamEntity>> LeaveTeam(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _teamManageService.LeaveTeamAsync(userId, id);

            return Accepted();
        }

    }
}
