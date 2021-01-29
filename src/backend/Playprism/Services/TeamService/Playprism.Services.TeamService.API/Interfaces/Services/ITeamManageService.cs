﻿using Playprism.Services.TeamService.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Interfaces.Services
{
    public interface ITeamManageService
    {
        Task<IEnumerable<TeamEntity>> GetTeamsAsync(int userId);
        Task<TeamEntity> GetTeamAsync(int id);
        Task<TeamEntity> AddTeamAsync(TeamEntity entity);
        Task<TeamEntity> UpdateTeamAsync(TeamEntity entity);
        Task DeleteTeamAsync(TeamEntity entity);
        Task InvitePlayerAsync(int id, string username);
        Task JoinTeamAsync(int userId, int teamId);
        Task RefuseTeamAsync(int userId, int teamId);
        Task<TeamEntity> LeaveTeamAsync(int userId, int teamId);
    }
}