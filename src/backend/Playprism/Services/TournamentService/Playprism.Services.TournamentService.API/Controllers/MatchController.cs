using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetIncomingMatches()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var matches = await _matchService.GetIncomingMatchListAsync(userId);
            if (matches == null)
            {
                return NotFound();
            }

            return Ok(matches);
        }

        [HttpPost("{id}/confirm")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ConfirmMatchResult(int id)
        {
            await _matchService.ConfirmMatchAsync(id);
            return Accepted();
        }

        [HttpPost("{id}/submitResult")]
        [ProducesResponseType(typeof(MatchEntity), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MatchEntity>> SubmitResult(int id, [FromBody] MatchEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }
            if (id != entity.Id)
            {
                return BadRequest();
            }

            try
            {
                var result = await _matchService.SetResultAsync(entity);
                return Accepted(result);
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] MatchEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            if (entity.Id != id)
            {
                return BadRequest();
            }
            // todo: add exceptionGlobalFilter
            // todo: check that user can update result of a given match
            await _matchService.UpdateMatchAsync(entity);

            return NoContent();
        }
    }

}