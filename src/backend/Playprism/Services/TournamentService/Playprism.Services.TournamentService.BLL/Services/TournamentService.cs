using System;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using Playprism.Services.TournamentService.BLL.Exceptions;
using System.Collections.Generic;
using Playprism.Services.TournamentService.BLL.Dtos;
using System.Linq;

namespace Playprism.Services.TournamentService.BLL.Services
{
    internal class TournamentService: ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IRepository<DisciplineEntity> _disciplineRepository;
        private readonly IMatchSettingsService _matchSettingsService;
        private readonly IMapper _mapper;

        public TournamentService(ITournamentRepository tournamentRepository, 
            IParticipantRepository participantRepository,
            IRepository<DisciplineEntity> disciplineRepository,
            IMatchSettingsService matchSettingsService,
            IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _participantRepository = participantRepository;
            _disciplineRepository = disciplineRepository;
            _matchSettingsService = matchSettingsService;
            _mapper = mapper;
        }

        public async Task<TournamentEntity> AddTournamentAsync(TournamentEntity entity)
        {
            if (entity.MaxNumberOfPlayers <= 1)
            {
                throw new ArgumentException("Number of players in tournament should be >= 2");
            }
            entity.Aborted = false;
            entity.Ongoing = false;
            entity.Published = false;
            entity.Finished = false;
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

        public async Task<TournamentDetailsResponse> GetTournamentDetailsAsync(int id)
        {
            var entity = (await _tournamentRepository.GetAsync(
                x => x.Id == id,
                includes: new string[] { "Participants", "Discipline" }
            )).FirstOrDefault();

            return _mapper.Map<TournamentDetailsResponse>(entity);
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

        public async Task<IEnumerable<TournamentListItemResponse>> GetTournamentsByDisciplineAsync(int disciplineId)
        {
            var entities = await _tournamentRepository.GetAsync(
                x => x.DisciplineId == disciplineId, 
                includes: new string[] { "Participants", "Discipline" });

            return _mapper.Map<IEnumerable<TournamentListItemResponse>>(entities);
        }
    }
}
