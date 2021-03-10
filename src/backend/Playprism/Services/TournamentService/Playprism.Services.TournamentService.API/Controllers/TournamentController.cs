using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.BLL.Exceptions;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer;
using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;
        private readonly ICompetitionProcess _competitionProcess;
        private readonly IAuthorizationService _authorizationService;

        public TournamentController(ITournamentService tournamentService, ICompetitionProcess competitionProcess, IAuthorizationService authorizationService)
        {
            _tournamentService = tournamentService;
            _competitionProcess = competitionProcess;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TournamentEntity), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TournamentEntity>>> Get([FromQuery] int disciplineId)
        {
            var entities = await _tournamentService.GetTournamentsByDiscipline(disciplineId);
            return Ok(entities);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TournamentEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TournamentEntity>> GetById(int id)
        {
            var entity = await _tournamentService.GetTournamentAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(TournamentEntity), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TournamentEntity>> Post([FromBody] TournamentEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            entity.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _tournamentService.AddTournamentAsync(entity);
            return Accepted(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TournamentEntity>> Put(int id, [FromBody] TournamentEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }
            if (id != entity.Id)
            {
                return BadRequest();
            }

            var tournament = _tournamentService.GetTournamentAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, tournament, "TournamentOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            try
            {
                await _tournamentService.UpdateTournamentAsync(entity);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> Delete(int id)
        {
            var tournament = _tournamentService.GetTournamentAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, tournament, "TournamentOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            await _tournamentService.DeleteTournamentAsync(id);
            return Accepted();
        }

        [Authorize]
        [HttpPost("join")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> JoinTournament(int id, [FromBody]ParticipantEntity participantEntity)
        {
            if (participantEntity == null)
            {
                return BadRequest();
            }
            try
            {
                participantEntity.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _tournamentService.AddNewParticipantAsync(participantEntity);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            return Accepted();
        }

        [Authorize]
        [HttpPost("{id}/start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> StartTournament(int id)
        {
            var tournament = await _tournamentService.GetTournamentAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, tournament, "TournamentOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            if (tournament == null)
            {
                return NotFound();
            }

            await _competitionProcess.GenerateBracketAsync(tournament);

            return Ok();
        }

        [Authorize]
        [HttpPost("{id}/rounds/{roundId}/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CloseRound(int id, int roundId)
        {
            var tournament = await _tournamentService.GetTournamentAsync(id);
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, tournament, "TournamentOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }
            try
            {
                await _competitionProcess.FinishRoundAsync(roundId);
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet("{id}/bracket")]
        [ProducesResponseType(typeof(BracketResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<BracketResponse>> GetBracket(int id)
        {
            var bracket = await _competitionProcess.GenerateResponseBracketAsync(id);
            return Ok(bracket);
        }
    }
}
