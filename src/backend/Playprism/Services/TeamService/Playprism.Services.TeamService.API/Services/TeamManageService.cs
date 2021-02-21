using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interfaces.Repositories;
using Playprism.Services.TeamService.API.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Playprism.Services.TeamService.API.Services
{
    public class TeamManageService : ITeamManageService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public TeamManageService(ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        public async Task<TeamEntity> AddTeamAsync(TeamEntity entity)
        {
            var duplicate = await _teamRepository.GetAsync(x => x.Name == entity.Name);
            if (duplicate != null)
            {
                throw new ValidationException("Team with provided name already exists");
            }

            await _teamRepository.AddAsync(entity);
            return entity;
        }

        public async Task DeleteTeamAsync(TeamEntity entity)
        {
            await _teamRepository.DeleteAsync(entity);
        }

        public async Task<TeamEntity> GetTeamAsync(int id)
        {
            return await _teamRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TeamEntity>> GetTeamsAsync(string userId)
        {
            var teams = await _teamRepository.GetAsync(x => x.OwnerId == userId);
            var memberTeams = await _playerRepository.GetMemberTeamsAsync(userId);

            teams.ToList().AddRange(memberTeams);
            return teams;
        }

        public Task InvitePlayerAsync(int id, string username)
        {
            throw new NotImplementedException();
        }

        public Task JoinTeamAsync(string userId, int teamId)
        {
            throw new NotImplementedException();
        }

        public Task<TeamEntity> LeaveTeamAsync(string userId, int teamId)
        {
            throw new NotImplementedException();
        }

        public Task RefuseTeamAsync(string userId, int teamId)
        {
            throw new NotImplementedException();
        }

        public Task<TeamEntity> UpdateTeamAsync(TeamEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
