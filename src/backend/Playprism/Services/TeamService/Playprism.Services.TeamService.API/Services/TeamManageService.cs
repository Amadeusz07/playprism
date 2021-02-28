using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Interface.Repositories;
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
        private readonly IAuth0Repository _auth0Repository;
        private readonly IPlayerService _playerService;
        private readonly ITeamPlayerAssignmentRepository _teamPlayerAssignmentRepository;

        public TeamManageService(ITeamRepository teamRepository, IPlayerRepository playerRepository, IAuth0Repository auth0Repository, IPlayerService playerService, ITeamPlayerAssignmentRepository teamPlayerAssignmentRepository)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _auth0Repository = auth0Repository;
            _playerService = playerService;
            _teamPlayerAssignmentRepository = teamPlayerAssignmentRepository;
        }

        public async Task<TeamEntity> AddTeamAsync(TeamEntity entity)
        {
            var duplicate = await _teamRepository.GetAsync(x => x.Name == entity.Name);
            if (duplicate.Any())
            {
                throw new ValidationException("Team with provided name already exists");
            }

            entity.CreateDate = DateTime.UtcNow;
            entity.Active = true;
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

        public async Task InvitePlayerAsync(int id, string username)
        {
            var user = await _auth0Repository.SearchUserByNameAsync(username);
            if (user != null)
            {
                var player = await _playerService.GetPlayerByUserInfoAsync(user);
                if (!await CanAssign(id, player.Id))
                {
                    throw new ValidationException("Active assignment already exists");
                }

                var assignment = new TeamPlayerAssignmentEntity()
                {
                    PlayerId = player.Id,
                    TeamId = id,
                    InviteDate = DateTime.UtcNow,
                    ResponseDate = null,
                    LeaveDate = null,
                    Accepted = false,
                    Active = false
                };

                await _teamPlayerAssignmentRepository.AddAsync(assignment);
            }
        }

        private async Task<bool> CanAssign(int teamId, int playerId)
        {
            var assignments = await _teamPlayerAssignmentRepository
                .GetAsync(x => x.TeamId == teamId &&
                    x.PlayerId == playerId &&
                    (x.Active || !x.Active && x.LeaveDate != null));
            return !assignments.Any();
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
