using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer
{
    public interface IShuffler
    {
        Task<IEnumerable<MatchEntity>> ShuffleAsync(IEnumerable<MatchEntity> matches, IEnumerable<int?> participantIds);
    }
}
