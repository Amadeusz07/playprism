using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Playprism.Services.TournamentService.BLL.Exceptions;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Playprism.Services.TournamentService.BLL.Dtos;

namespace Playprism.Services.TournamentService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/tournament/{tournamentId}/[controller]")]
    public class MatchDefinitionController: ControllerBase
    {
        private readonly IMatchSettingsService _matchSettingsService;
        private readonly ITournamentService _tournamentService;
        private readonly IAuthorizationService _authorizationService;

        public MatchDefinitionController(IMatchSettingsService matchSettingsService, ITournamentService tournamentService, IAuthorizationService authorizationService)
        {
            _matchSettingsService = matchSettingsService;
            _tournamentService = tournamentService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MatchDefinitionEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MatchDefinitionEntity>>> GetByTournamentId(int tournamentId)
        {
            var tournament = await _tournamentService.GetTournamentAsync(tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, tournament, "TournamentOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var entities = await _matchSettingsService.GetMatchSettingsByTournamentAsync(tournamentId);
            if (entities == null)
            {
                return NotFound();
            }
            return Ok(entities);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MatchDefinitionEntity), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MatchDefinitionEntity>> Post(int tournamentId, [FromBody] MatchDefinitionEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
            }

            var tournament = await _tournamentService.GetTournamentAsync(tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, tournament, "TournamentOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            try
            {
                entity = await _matchSettingsService.AddMatchSettingsAsync(entity);
            }
            catch (ValidationException)
            {
                return BadRequest("Name already exists");
            }
            
            return Accepted(entity);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int tournamentId, int id, [FromBody] UpdateMatchDefinitionRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var tournament = await _tournamentService.GetTournamentAsync(tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, tournament, "TournamentOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            try
            {
                await _matchSettingsService.UpdateMatchSettingsAsync(id, request);
            }
            catch (EntityNotFoundException)
            {
                return NotFound($"Not found team with id {id}");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int tournamentId, int id)
        {
            var tournament = await _tournamentService.GetTournamentAsync(tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, tournament, "TournamentOwnerPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            try
            {
                await _matchSettingsService.DeleteMatchSettingsAsync(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return Accepted();
        }
        
    }
}