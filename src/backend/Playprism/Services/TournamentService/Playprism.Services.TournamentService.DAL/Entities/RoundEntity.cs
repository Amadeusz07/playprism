using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Playprism.Services.TournamentService.DAL.Entities
{
    public class RoundEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }
        [ForeignKey("MatchDefinition")]
        public int MatchDefinitionId { get; set; }
        public int Order { get; set; }
        public int NumberOfCompetitors { get; set; }
        public int NumberOfMatches { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Started { get; set; }
        public bool Finished { get; set; }

        public virtual MatchDefinitionEntity MatchDefinition { get; set; }
        public ICollection<MatchEntity> Matches { get; set; }
        
        public Dictionary<int, int?> GetWinnersByMatchId()
        {
            if (!Finished)
            {
                throw new InvalidOperationException($"Round {Id} is not finished");
            }

            if (Matches.Any(x => !x.Played || !x.Confirmed))
            {
                throw new InvalidOperationException($"Not all matches in round {Id} are played and confirmed");
            }

            if (Matches.Any(x => x.Result == null))
            {
                throw new InvalidOperationException($"Round {Id} has null match results");
            }

            var winners = new Dictionary<int, int?>();
            foreach (var match in Matches)
            {
                switch (match.Result)
                {
                    case 1:
                        winners.Add(match.Id, match.Participant1Id.Value);
                        break;
                    case 2:
                        winners.Add(match.Id, match.Participant2Id.Value);
                        break;
                    default:
                        winners.Add(match.Id, null);
                        break;
                }
            }
            return winners;

        }
    }
}
