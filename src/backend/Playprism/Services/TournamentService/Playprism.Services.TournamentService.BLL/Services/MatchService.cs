using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.BLL.Exceptions;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

namespace Playprism.Services.TournamentService.BLL.Services
{
    public class MatchService: IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;
        private const string EmptySlot = "EMPTY";

        public MatchService(IMatchRepository matchRepository, IParticipantRepository participantRepository, IMapper mapper)
        {
            _matchRepository = matchRepository;
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MatchDto>> GetIncomingMatchListAsync(string userId)
        {
            var participants = await _participantRepository.GetParticipantsByUserIdAsync(userId);
            if (participants == null)
            {
                return null;
            }
            List<MatchDto> result = new List<MatchDto>();
            foreach (var participant in participants)
            {
                var entities = await _matchRepository.GetIncomingMatchesAsync(participant.Id);
                var matches = _mapper.Map<IEnumerable<MatchDto>>(entities);
                result.AddRange(matches);
            }

            if (result.Count == 0)
            {
                return null;
            }
            await FillParticipantsNamesAsync(result);
            return result.OrderBy(x => x.MatchDate);
        }

        private async Task<IEnumerable<MatchDto>> FillParticipantsNamesAsync(IEnumerable<MatchDto> matches)
        {
            var participantIds = matches
                .Where(x => x.Participant1Id != null)
                .Select(x => x.Participant1Id.Value)
                .Concat(
                    matches
                        .Where(x => x.Participant2Id != null)
                        .Select(x => x.Participant2Id.Value)
                ).Distinct();
            var neededParticipants = await _participantRepository.GetByIdsAsync(participantIds);

            foreach (var match in matches)
            {
                SetNames(neededParticipants, match);
            }

            return matches;
        }

        private void SetNames(IEnumerable<ParticipantEntity> participants, MatchDto match)
        {
            var participant1 = participants.FirstOrDefault(x => x.Id == match.Participant1Id);
            match.Participant1Name = participant1 != null ? participant1.Name : EmptySlot;
            var participant2 = participants.FirstOrDefault(x => x.Id == match.Participant2Id);
            match.Participant2Name = participant2 != null ? participant2.Name : EmptySlot;
        }

        public async Task<MatchEntity> UpdateMatchAsync(MatchEntity entity)
        {
            var match = await _matchRepository.GetByIdAsync(entity.Id);
            if (match == null)
            {
                throw new EntityNotFoundException();
            }

            match = _mapper.Map(entity, match);
            await _matchRepository.UpdateAsync(match);
            return match;
        }

        public async Task<MatchEntity> ConfirmMatchAsync(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
            {
                throw new EntityNotFoundException();
            }

            if (match.Result == null || match.Participant1Score == null || match.Participant2Score == null)
            {
                throw new InvalidOperationException($"Cannot confirm match {id} that has no results set");
            }

            match.Confirmed = true;
            await _matchRepository.UpdateAsync(match); 
            return match;
        }

        public async Task<MatchEntity> SetResultAsync(MatchEntity result)
        {
            var match = await _matchRepository.GetFullByIdAsync(result.Id);
            if (match == null)
            {
                throw new EntityNotFoundException();
            }

            if (match.Played || match.Result != null || match.Participant1Score != null || match.Participant2Score != null)
            {
                throw new InvalidOperationException($"Result of match {result.Id} already set");
            }

            if (IsMatchResultValid(result, match.Round.MatchDefinition))
            {
                if (!match.Round.MatchDefinition.ConfirmationNeeded)
                {
                    match.Confirmed = true;
                }

                match.Played = true;
                match.Result = result.Result;
                match.Participant1Score = result.Participant1Score;
                match.Participant2Score = result.Participant2Score;
                await _matchRepository.UpdateAsync(match);
            }

            return match;
        }
        
        // for now public, might be used to valid on a fly data in frontend
        public bool IsMatchResultValid(MatchEntity match, MatchDefinitionEntity matchDefinition)
        {
            // todo: it shouldn't be the part of this method, fits more likely to MatchDefinition CRUD methods
            // if (matchDefinition.NumberOfGames % 2 != 1)
            // {
            //     throw new ArgumentException("Number of games should be not divisible by two");    
            // }
            
            if (match.Participant1Score < 0 || match.Participant2Score < 0)
            {
                throw new ValidationException($"One of scores of match {match.Id} is less that 0");
            }

            if (!matchDefinition.ScoreBased)
            {
                if (match.Result == 0)
                {
                    throw new ValidationException($"Result of match {match.Id} cannot be a draw in ScoreBased set to false");
                }
                var correctWinnerScore = (matchDefinition.NumberOfGames / 2) + 1;
                if (match.Result == 1 && match.Participant1Score != correctWinnerScore)
                {
                    throw new ValidationException($"Participant1 score in match {match.Id} is not correct");
                }

                if (match.Result == 2 && match.Participant2Score != correctWinnerScore)
                {
                    throw new ValidationException($"Participant2 score in match {match.Id} is not correct");
                }
            }

            switch (match.Result)
            {
                case 0:
                    if (match.Participant1Score != match.Participant2Score)
                        throw new ValidationException($"Scores are not representing result of match {match.Id}");
                    break;
                case 1:
                    if (match.Participant1Score <= match.Participant2Score)
                        throw new ValidationException($"Scores are not representing result of match {match.Id}");
                    break;
                case 2:
                    if (match.Participant1Score >= match.Participant2Score)
                        throw new ValidationException($"Scores are not representing result of match {match.Id}");
                    break;
                default:
                    throw new ValidationException($"Result of match {match.Id} is not correct");
            }

            return true;

        }
    }
}