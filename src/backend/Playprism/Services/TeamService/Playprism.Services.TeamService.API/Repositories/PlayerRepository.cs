using Microsoft.EntityFrameworkCore;
using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Repositories
{
    internal class PlayerRepository: Repository<PlayerEntity>, IPlayerRepository
    {
        public PlayerRepository(TeamDbContext mainDbContext) : base(mainDbContext)
        {
        }

        public async Task<IEnumerable<TeamEntity>> GetMemberTeamsAsync(int userId)
        {
            var player = await MainDbContext.Players
                .Include(x => x.Assignments)
                .ThenInclude(x => x.Team)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return player.Assignments.Select(x => x.Team);
        }
    }
}
