using Microsoft.EntityFrameworkCore;
using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Repositories
{
    public class TeamPlayerAssignmentRepository: Repository<TeamPlayerAssignmentEntity>, ITeamPlayerAssignmentRepository
    {
        public TeamPlayerAssignmentRepository(TeamDbContext mainDbContext) : base(mainDbContext)
        {
        }

        /// <summary>
        /// Gets pending and active assignments
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TeamPlayerAssignmentEntity>> GetAssignmentsAsync(string userId)
        {
            var player = MainDbContext.Players.FirstOrDefault(x => x.UserId == userId);
            if (player == null)
            {
                return null;
            }

            return await MainDbContext.TeamPlayerAssignments
                .Include(x => x.Team)
                .Where(x => 
                    (x.PlayerId == player.Id) &&
                    (x.Active || (!x.Active && x.ResponseDate == null))
                )
                .ToListAsync();
        }

        public async Task DeleteAssignments(int teamId)
        {
            var toRemove = MainDbContext.TeamPlayerAssignments.Where(x => x.TeamId == teamId);
            MainDbContext.TeamPlayerAssignments.RemoveRange(toRemove);
            await MainDbContext.SaveChangesAsync();
        }

    }
}
