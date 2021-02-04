using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Services
{
    internal class BrowserService : IBrowserService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IRepository<DisciplineEntity> _disciplineRepository;

        public BrowserService(ITournamentRepository tournamentRepository, IRepository<DisciplineEntity> disciplineRepository)
        {
            _tournamentRepository = tournamentRepository;
            _disciplineRepository = disciplineRepository;
        }

        public async Task<IEnumerable<DisciplineEntity>> GetDisciplinesByPopularity(int? count = null)
        {
            var tournaments = await _tournamentRepository.GetAsync(includeString: "Discipline");
            IEnumerable<DisciplineEntity> disciplinesByPopularity = tournaments
                    .GroupBy(x => x.DisciplineId)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.First().Discipline);

            var emptyDisciplines = await _disciplineRepository.GetAsync(x => !disciplinesByPopularity.Select(r => r.Id).Contains(x.Id));
            emptyDisciplines = emptyDisciplines.OrderBy(x => x.Name).ToList();
            disciplinesByPopularity = disciplinesByPopularity.Concat(emptyDisciplines);

            if (count.HasValue)
            {
                return disciplinesByPopularity.Take(count.Value);
            }
            else
            {
                return disciplinesByPopularity;
            }
        }
    }
}
