using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Playprism.Services.TournamentService.BLL.Exceptions;
using Playprism.Services.TournamentService.BLL.Interfaces;
using Playprism.Services.TournamentService.DAL.Entities;
using Playprism.Services.TournamentService.DAL.Interfaces;

namespace Playprism.Services.TournamentService.BLL.Services
{
    public class MatchService: IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<MatchEntity> UpdateMatchAsync(MatchEntity entity)
        {
            var match = await _matchRepository.GetByIdAsync(entity.Id);
            if (match == null)
            {
                throw new EntityNotFoundException();
            }
            var result = await _matchRepository.UpdateAsync(entity);
            return result;
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

        public async Task<MatchEntity> SetResultAsync(MatchEntity entity)
        {
            var match = await _matchRepository.GetByIdAsync(entity.Id);
            if (match == null)
            {
                throw new EntityNotFoundException();
            }

            if (match.Played || match.Result != null || match.Participant1Score != null || match.Participant2Score != null)
            {
                throw new InvalidOperationException($"Result of match {entity.Id} already set");
            }

            if (IsMatchResultValid(entity, match.Round.MatchDefinition))
            {
                if (!match.Round.MatchDefinition.ConfirmationNeeded)
                {
                    match.Confirmed = true;
                }

                match.Played = true;
                return await _matchRepository.UpdateAsync(match);
            }
            else
            {
                throw new ValidationException("");
            }
            
        }
        
        // for now public, might be used to valid on a fly data in frontend
        public bool IsMatchResultValid(MatchEntity match, MatchDefinitionEntity matchDefinition)
        {
            if (matchDefinition.NumberOfGames % 2 != 1)
            {
                throw new ArgumentException("Number of games should be not divisible by two");    
            }
            
            if (match.Participant1Score > 0 || match.Participant2Score > 0)
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
                        throw new ValidationException($"Scores are not presenting result of match {match.Id}");
                    break;
                case 1:
                    if (match.Participant1Score <= match.Participant2Score)
                        throw new ValidationException($"Scores are not presenting result of match {match.Id}");
                    break;
                case 2:
                    if (match.Participant1Score >= match.Participant2Score)
                        throw new ValidationException($"Scores are not presenting result of match {match.Id}");
                    break;
                default:
                    throw new ArgumentException($"Result of match {match.Id} is not correct");
            }

            return true;

        }
    }
}