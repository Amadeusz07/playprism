using System.Threading.Tasks;
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

        [HttpPut("{id}")]
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
            await _matchService.UpdateMatchAsync(entity);

            return NoContent();
        }
    }

}