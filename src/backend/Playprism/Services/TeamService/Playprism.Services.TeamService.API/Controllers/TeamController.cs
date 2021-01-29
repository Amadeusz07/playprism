﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamManageService _teamManageService;

        public TeamController(ITeamManageService teamManageService)
        {
            _teamManageService = teamManageService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TeamEntity>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TeamEntity>>> Get()
        {
            int userId = new Random().Next(10); // todo: get userId from token
            var entities = await _teamManageService.GetTeamsAsync(userId);

            return Ok(entities);
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
            entity = await _teamManageService.AddTeamAsync(entity);

            return Accepted(entity);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put(int id, [FromBody] TeamEntity entity)
        {
            await _teamManageService.UpdateTeamAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> Delete(int id)
        {
            var team = await _teamManageService.GetTeamAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            await _teamManageService.DeleteTeamAsync(team);

            return Accepted();
        }

        [HttpPost("{id}/invite/{username}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> InvitePlayerToTeam(int id, string username)
        {
            await _teamManageService.InvitePlayerAsync(id, username);

            return Accepted();
        }


        [HttpPost("{id}/join")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<TeamEntity>> JoinTeam(int id)
        {
            int userId = new Random().Next(10); // todo: get userId from token
            await _teamManageService.JoinTeamAsync(userId, id);

            return Accepted();
        }

        [HttpPost("{id}/refuse")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> RefuseTeam(int id)
        {
            int userId = new Random().Next(10); // todo: get userId from token
            await _teamManageService.JoinTeamAsync(userId, id);

            return Accepted();
        }

        [HttpPost("{id}/leave")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult<TeamEntity>> LeaveTeam(int id)
        {
            int userId = new Random().Next(10); // todo: get userId from token
            await _teamManageService.LeaveTeamAsync(userId, id);

            return Accepted();
        }

    }
}