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
using System.ComponentModel.DataAnnotations;

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

        public async Task<TournamentEntity> AddTournamentAsync(CreateTournamentRequest request)
        {
            if (string.IsNullOrEmpty(request.OwnerName))
            {
                // TODO: should reach out to auth0 for a user's username
                throw new ValidationException("No owner name provided");
            }
            if (request.MaxNumberOfPlayers <= 1)
            {
                throw new ArgumentException("Number of players in tournament should be >= 2");
            }
            var entity = _mapper.Map<TournamentEntity>(request);
            entity.Aborted = false;
            entity.Ongoing = false;
            entity.Published = false;
            entity.Finished = false;
            entity.Timezone = "UTC";
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

        public async Task<TournamentEntity> UpdateTournamentAsync(int id, UpdateTournamentRequest request)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(id);
            if (tournament == null)
            {
                throw new EntityNotFoundException();
            }

            tournament = _mapper.Map(request, tournament);
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
            var canJoin = await CanJoinTournament(
                new JoinTournamentRequest { CandidateId = tournament.AreTeams ? entity.TeamId : entity.PlayerId, Name = entity.Name },
                tournament.Id);
            if (!canJoin.Accepted)
            {
                throw new ValidationException(canJoin.Message);
            }
            entity.Approved = !tournament.RegistrationApprovalNeeded;
            entity.RegistrationDate = DateTime.UtcNow;
            var result = await _participantRepository.AddAsync(entity);
            return result;
        }

        public async Task<IEnumerable<TournamentListItemResponse>> GetTournamentsByDisciplineAsync(int disciplineId)
        {
            var entities = await _tournamentRepository.GetAsync(
                x => x.DisciplineId == disciplineId && x.Published, 
                includes: new string[] { "Participants", "Discipline" });

            return _mapper.Map<IEnumerable<TournamentListItemResponse>>(entities);
        }

        public async Task<CanJoinResponse> CanJoinTournament(JoinTournamentRequest joinRequest, int tournamentId)
        {
            var tournament = (await _tournamentRepository.GetAsync(x => x.Id == tournamentId, includeString: "Participants")).First();
            if (tournament.Participants.Count >= tournament.MaxNumberOfPlayers)
            {
                return new CanJoinResponse { Accepted = false, Message = "Tournament is full" };
            }
            if (tournament.AreTeams)
            {
                var registration = tournament.Participants.FirstOrDefault(x => x.TeamId == joinRequest.CandidateId);
                if (registration != null)
                {
                    var message = registration.Approved ? 
                        "Your team has been added to the tournament" : 
                        "Register request sent to the tournament\'s organizer";
                    return new CanJoinResponse { Accepted = false, Message = message };
                }
            }
            else
            {
                var registration = tournament.Participants.FirstOrDefault(x => x.PlayerId == joinRequest.CandidateId);
                if (registration != null)
                {
                    var message = registration.Approved ?
                        "You have been added to the tournament" :
                        "Register request sent to the tournament\'s organizer";
                    return new CanJoinResponse { Accepted = false, Message = message };
                }
            }

            return new CanJoinResponse { Accepted = true, Message = "Ok" };
        }
    }
}
