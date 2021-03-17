using AutoMapper;
using Playprism.Services.TeamService.API.Dtos;
using Playprism.Services.TeamService.API.Entities;
using Playprism.Services.TeamService.API.Exceptions;
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
        private readonly IMapper _mapper;

        public TeamManageService(ITeamRepository teamRepository, IPlayerRepository playerRepository, IAuth0Repository auth0Repository, IPlayerService playerService, ITeamPlayerAssignmentRepository teamPlayerAssignmentRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _auth0Repository = auth0Repository;
            _playerService = playerService;
            _teamPlayerAssignmentRepository = teamPlayerAssignmentRepository;
            _mapper = mapper;
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
            var memberTeams = (await _playerRepository.GetMemberTeamsAsync(userId)).ToList();
            teams = teams.Concat(memberTeams).ToList();
            return teams;
        }

        public async Task<TeamEntity> GetCurrentTeamAsync(string userId)
        {
            var team = (await _teamRepository.GetAsync(x => x.OwnerId == userId)).FirstOrDefault();
            if (team == null)
            {
                var assignments = await _teamPlayerAssignmentRepository.GetAssignmentsAsync(userId);
                team = assignments.FirstOrDefault(x => x.Active)?.Team;
                if (team != null)
                {
                    team.Assignments = null; // TODO: create fix => DTO
                }
            }
            return team;
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAssignments(string userId)
        {
            var assignments = await _teamPlayerAssignmentRepository.GetAssignmentsAsync(userId);
            return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
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

        public async Task JoinTeamAsync(string userId, int teamId)
        {
            var assignment = await GetAndValidateAssignmentAsync(userId, teamId);

            assignment.Accepted = true;
            assignment.Active = true;
            assignment.ResponseDate = DateTime.Now;

            await _teamPlayerAssignmentRepository.UpdateAsync(assignment);
        }

        private async Task<TeamPlayerAssignmentEntity> GetAndValidateAssignmentAsync(string userId, int teamId)
        {
            var entities = await _playerRepository.GetAsync(x => x.UserId == userId, includeString: "Assignments");
            if (!entities.Any())
            {
                throw new EntityNotFoundException($"Player not found");
            }
            var player = entities.First();
            if (!player.Assignments.Any())
            {
                throw new EntityNotFoundException($"Player\'s assignments not found");
            }
            var isActiveMemberOfAnyTeam = player.Assignments.Any(x => x.Active);
            if (isActiveMemberOfAnyTeam)
            {
                throw new ValidationException("User already has team");
            }
            var assignment = player.Assignments.SingleOrDefault(x => x.TeamId == teamId);
            if (assignment == null)
            {
                throw new EntityNotFoundException($"Player\'s assignment to team {teamId} not found");
            }

            return assignment;
        }

        public async Task LeaveTeamAsync(string userId, int teamId)
        {
            var assignment = await GetAssignmentAsync(userId, teamId);
            if (!assignment.Active || !assignment.Accepted)
            {
                throw new ValidationException("Assignment is not active already or was not accepted beforehand");
            }

            assignment.Active = false;
            assignment.LeaveDate = DateTime.UtcNow;
            await _teamPlayerAssignmentRepository.UpdateAsync(assignment);
        }

        public async Task RefuseTeamAsync(string userId, int teamId)
        {
            var assignment = await GetAssignmentAsync(userId, teamId);
            if (assignment.Active || assignment.Accepted || assignment.ResponseDate != null)
            {
                throw new ValidationException("Can\'t refuse already active or accepted assignment");
            }

            assignment.ResponseDate = DateTime.UtcNow;
            await _teamPlayerAssignmentRepository.UpdateAsync(assignment);
        }

        private async Task<TeamPlayerAssignmentEntity> GetAssignmentAsync(string userId, int teamId)
        {
            var player = (await _playerRepository.GetAsync(x => x.UserId == userId, includeString: "Assignments")).FirstOrDefault();
            if (player == null)
            {
                throw new EntityNotFoundException("Player not found");
            }
            var assignment = player.Assignments.FirstOrDefault(x => x.TeamId == teamId);
            if (assignment == null)
            {
                throw new EntityNotFoundException("Assignment not found");
            }

            return assignment;
        }

        public Task<TeamEntity> UpdateTeamAsync(TeamEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
