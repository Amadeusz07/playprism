﻿using System;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using Playprism.Services.TournamentService.BLL.Exceptions;

namespace Playprism.Services.TournamentService.BLL.Services
{
    internal class CrudService: ICrudService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMatchSettingsService _matchSettingsService;
        private readonly IMapper _mapper;

        public CrudService(ITournamentRepository tournamentRepository, 
            IParticipantRepository participantRepository, 
            IMatchSettingsService matchSettingsService,
            IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _participantRepository = participantRepository;
            _matchSettingsService = matchSettingsService;
            _mapper = mapper;
        }

        public async Task<TournamentEntity> AddTournamentAsync(TournamentEntity entity)
        {
            var result = await _tournamentRepository.AddAsync(entity);
            await _matchSettingsService.CreateDefaultSettingsAsync(result.Id);
            
            return result;
        }

        public async Task DeleteTournamentAsync(int id)
        {
            var entity = await _tournamentRepository.GetByIdAsync(id);
            await _tournamentRepository.DeleteAsync(entity);
        }

        public async Task<TournamentEntity> GetTournamentAsync(int id)
        {
            return await _tournamentRepository.GetByIdAsync(id);
        }

        public async Task<TournamentEntity> UpdateTournamentAsync(TournamentEntity entity)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(entity.Id);
            if (tournament == null)
            {
                throw new EntityNotFoundException();
            }

            tournament = _mapper.Map(entity, tournament);
            await _tournamentRepository.UpdateAsync(tournament);
            return tournament;
        }

        public async Task<ParticipantEntity> AddNewParticipantAsync(ParticipantEntity entity)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(entity.TournamentId);
            if (tournament == null)
            {
                throw new EntityNotFoundException();
            }
            
            entity.Approved = !tournament.RegistrationApprovalNeeded;
            entity.RegistrationDate = DateTime.UtcNow;
            var result = await _participantRepository.AddAsync(entity);
            return result;
        }
        
    }
}
