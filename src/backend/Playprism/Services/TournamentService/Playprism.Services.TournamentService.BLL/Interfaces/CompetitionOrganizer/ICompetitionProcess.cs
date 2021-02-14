using System.Threading.Tasks;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.BLL.Interfaces.CompetitionOrganizer
{
    public interface ICompetitionProcess
    {
        Task GenerateBracketAsync(TournamentEntity tournament);
        Task FinishRoundAsync(int roundId);
        Task<BracketResponse> GenerateResponseBracketAsync(int tournamentId);
    }
}