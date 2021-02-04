using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Interfaces
{
    public interface IBrowserService
    {
        Task<IEnumerable<DisciplineEntity>> GetDisciplinesByPopularity(int? count = null);
    }
}
