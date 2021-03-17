using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlayerController: ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PlayerEntity), StatusCodes.Status200OK)]
        public async Task<ActionResult<PlayerEntity>> GetPlayerInfo()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _playerService.GetPlayerByUserIdAsync(userId);
            return Ok(result);
        }
    }
}
