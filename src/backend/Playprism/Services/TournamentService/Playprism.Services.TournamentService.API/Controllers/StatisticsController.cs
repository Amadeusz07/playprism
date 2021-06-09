using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IPlayerStatisticsService _playerStatisticsService;

        public StatisticsController(IPlayerStatisticsService playerStatisticsService)
        {
            _playerStatisticsService = playerStatisticsService;
        }

        [HttpGet("player")]
        public async Task<ActionResult<IEnumerable<PlayerRecordsResponse>>> GetPlayerStatistics()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var results = await _playerStatisticsService.GetPlayerRecords(userId);

            return Ok(results);
        }
    }
}
