using System;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using Playprism.Services.TournamentService.BLL.Exceptions;
using System.Collections.Generic;

namespace Playprism.Services.TournamentService.BLL.Services
{
    internal class TournamentService: ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMatchSettingsService _matchSettingsService;
        private readonly IMapper _mapper;

        public TournamentService(ITournamentRepository tournamentRepository, 
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
            if (entity.MaxNumberOfPlayers <= 1)
            {
                throw new ArgumentException("Number of players in tournament should be >= 2");
            }
            var result = await _tournamentRepository.AddAsync(entity);
            await _matchSettingsService.CreateDefaultSettingsAsync(result.Id);
            
            return result;
        }

        public async Task DeleteTournamentAsync(int id)
        {
            var entity = await _tournamentRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException();
            }
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

        public async Task<IEnumerable<TournamentEntity>> GetTournamentsByDiscipline(int disciplineId)
        {
            var tournaments = await _tournamentRepository.GetAsync(x => x.DisciplineId == disciplineId);
            return tournaments;
        }
    }
}
