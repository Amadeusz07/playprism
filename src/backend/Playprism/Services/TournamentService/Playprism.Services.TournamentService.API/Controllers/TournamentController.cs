﻿using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(TournamentListItemResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TournamentListItemResponse>>> Get([FromQuery] int disciplineId)
        {
            var entities = await _tournamentService.GetTournamentsByDisciplineAsync(disciplineId);
            return Ok(entities);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TournamentDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TournamentDetailsResponse>> GetById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            var entity = await _tournamentService.GetTournamentDetailsAsync(id);
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
            entity.OwnerName = User.FindFirst(ClaimTypes.Name).Value;
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
            if (tournament == null)
            {
                return NotFound();
            }

            await _tournamentService.UpdateTournamentAsync(entity);
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
        public async Task<ActionResult> JoinTournament(int id, [FromBody]ParticipantEntity participant)
        {
            if (participant == null)
            {
                return BadRequest();
            }
            if (participant.TournamentId == 0)
            {
                return BadRequest();
            }
            try
            {
                participant.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _tournamentService.AddNewParticipantAsync(participant);
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

        [Authorize]
        [HttpGet("{id}/can-join/{candidateId}")]
        [ProducesResponseType(typeof(CanJoinResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> CanJoinTournament(int id, int candidateId)
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _tournamentService.CanJoinTournament(new JoinTournamentRequest()
            {
                Name = null,
                CandidateId = candidateId
            }, id);

            return Ok(result);
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
