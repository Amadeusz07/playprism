using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.API.Controllers
{
    [ApiController]
    [Route("api/v1/tournament/{tournamentId}/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
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

            var result = await _matchService.SetResultAsync(entity);

            return Accepted(result);
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