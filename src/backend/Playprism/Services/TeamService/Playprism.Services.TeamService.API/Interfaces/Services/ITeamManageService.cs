using Playprism.Services.TeamService.API.Dtos;
using Playprism.Services.TeamService.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Interfaces.Services
{
    public interface ITeamManageService
    {
        Task<IEnumerable<TeamEntity>> GetTeamsAsync(string userId);
        Task<TeamEntity> GetTeamAsync(int id);
        Task<IEnumerable<AssignmentResponse>> GetAssignments(string userId);
        Task<TeamEntity> AddTeamAsync(TeamEntity entity);
        Task<TeamEntity> UpdateTeamAsync(TeamEntity entity);
        Task DeleteTeamAsync(TeamEntity entity);
        Task InvitePlayerAsync(int id, string username);
        Task JoinTeamAsync(string userId, int teamId);
        Task RefuseTeamAsync(string userId, int teamId);
        Task<TeamEntity> LeaveTeamAsync(string userId, int teamId);
    }
}
