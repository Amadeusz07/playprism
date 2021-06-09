using System.Collections.Generic;
using System.Threading.Tasks;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.BLL.Interfaces
{
    public interface IMatchSettingsService
    {
        Task<IEnumerable<MatchDefinitionEntity>> GetMatchSettingsByTournamentAsync(int tournamentId);
        Task<MatchDefinitionEntity> AddMatchSettingsAsync(MatchDefinitionEntity entity);
        Task<MatchDefinitionEntity> CreateDefaultSettingsAsync(int tournamentId);
        Task<MatchDefinitionEntity> UpdateMatchSettingsAsync(int id, UpdateMatchDefinitionRequest entity);
        Task DeleteMatchSettingsAsync(int id);
    }
}