using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Repositories;

namespace Playprism.Services.TeamService.API.Repositories
{
    internal class TeamRepository: Repository<TeamEntity>, ITeamRepository
    {
        public TeamRepository(TeamDbContext mainDbContext) : base(mainDbContext)
        {
        }

    }
}