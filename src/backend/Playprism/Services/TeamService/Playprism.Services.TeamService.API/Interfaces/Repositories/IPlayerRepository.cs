using Playprism.Services.TeamService.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Interfaces.Repositories
{
    public interface IPlayerRepository: IRepository<PlayerEntity>
    {
        Task<IEnumerable<TeamEntity>> GetMemberTeamsAsync(string userId);
    }
}
