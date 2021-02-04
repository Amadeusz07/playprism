using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DisciplineController : ControllerBase
    {
        private readonly IBrowserService _browserService;

        public DisciplineController(IBrowserService browserService)
        {
            _browserService = browserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisciplineEntity>>> Get([FromQuery] int? number)
        {
            var disciplines = await _browserService.GetDisciplinesByPopularity(number);

            return Ok(disciplines);
        }

    }
}
