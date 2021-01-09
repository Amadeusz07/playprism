using System.Threading.Tasks;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.BLL.Interfaces
{
    public interface IMatchService
    {
        Task<MatchEntity> UpdateMatchAsync(MatchEntity entity);
        Task<MatchEntity> ConfirmMatchAsync(int id);
        Task<MatchEntity> SetResultAsync(MatchEntity entity);
        // for now public, might be used to valid on a fly data in frontend
        bool IsMatchResultValid(MatchEntity match, MatchDefinitionEntity matchDefinitionEntity);
    }
}