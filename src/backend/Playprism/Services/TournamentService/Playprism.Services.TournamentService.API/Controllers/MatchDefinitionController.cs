using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Playprism.Services.TournamentService.BLL.Exceptions;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Playprism.Services.TournamentService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/tournament/{tournamentId}/[controller]")]
    public class MatchDefinitionController: ControllerBase
    {
        private readonly IMatchSettingsService _matchSettingsService;

        public MatchDefinitionController(IMatchSettingsService matchSettingsService)
        {
            _matchSettingsService = matchSettingsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MatchDefinitionEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MatchDefinitionEntity>>> GetByTournamentId(int tournamentId)
        {
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
        public async Task<ActionResult<MatchDefinitionEntity>> Post([FromBody] MatchDefinitionEntity entity)
        {
            if (entity == null)
            {
                return BadRequest();
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
        public async Task<ActionResult> Put(int id, [FromBody] MatchDefinitionEntity entity)
        {
            if (entity == null || entity.Id != id)
            {
                return BadRequest();
            }

            try
            {
                await _matchSettingsService.UpdateMatchSettingsAsync(entity);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(int id)
        {
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