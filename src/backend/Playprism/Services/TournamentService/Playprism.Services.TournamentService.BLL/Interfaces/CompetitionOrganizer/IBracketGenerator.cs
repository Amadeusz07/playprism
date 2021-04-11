using Playprism.Services.TournamentService.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer
{
    public interface IBracketGenerator
    {
        Task ClearBracketAsync(int tournamentId);
        Task<IEnumerable<RoundEntity>> GenerateRoundsAsync(TournamentEntity tournament, int participantsCount);
        Task<IEnumerable<MatchEntity>> GenerateMatchesAsync(IEnumerable<RoundEntity> rounds);
    }
}
