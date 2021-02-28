using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Repositories;

namespace Playprism.Services.TeamService.API.Repositories
{
    public class TeamPlayerAssignmentRepository: Repository<TeamPlayerAssignmentEntity>, ITeamPlayerAssignmentRepository
    {
        public TeamPlayerAssignmentRepository(TeamDbContext mainDbContext) : base(mainDbContext)
        {
        }

    }
}
