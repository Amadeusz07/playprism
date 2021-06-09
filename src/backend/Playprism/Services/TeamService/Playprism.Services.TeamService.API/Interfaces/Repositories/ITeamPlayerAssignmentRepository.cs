using Playprism.Services.TeamService.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Interfaces.Repositories
{
    public interface ITeamPlayerAssignmentRepository: IRepository<TeamPlayerAssignmentEntity>
    {
        Task<IEnumerable<TeamPlayerAssignmentEntity>> GetAssignmentsAsync(string userId);
        Task DeleteAssignments(int teamId);
    }
}
