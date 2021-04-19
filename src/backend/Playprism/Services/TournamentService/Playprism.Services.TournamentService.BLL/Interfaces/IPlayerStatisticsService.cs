using Playprism.Services.TournamentService.BLL.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TournamentService.BLL.Interfaces
{
    public interface IPlayerStatisticsService
    {
        Task<IEnumerable<PlayerRecordsResponse>> GetPlayerRecords(string userId);
    }
}
