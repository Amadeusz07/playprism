using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Playprism.Services.TournamentService.DAL.Entities
{
    public class MatchEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }
        [ForeignKey("Round")]
        public int RoundId { get; set; }
        public DateTime? MatchDate { get; set; }
        public int? Participant1Id { get; set; }
        public int? Participant2Id { get; set; }
        public int? PreviousMatch1Id { get; set; }
        public int? PreviousMatch2Id { get; set; }
        public int? Participant1Score { get; set; }
        public int? Participant2Score { get; set; }
        public int? Result { get; set; }
        public bool Played { get; set; }
        public bool Confirmed { get; set; }

        public virtual TournamentEntity Tournament { get; set; }
        [JsonIgnore]
        public virtual RoundEntity Round { get; set; }
        public ICollection<GameEntity> Games { get; set; }

        public void AutoResult()
        {
            if (Participant1Id != null || Participant2Id != null)
            {
                if (Participant1Id == null)
                {
                    Result = 2;
                }
                else if (Participant2Id == null)
                {
                    Result = 1;
                }
            }
            else
            {
                Result = -1;
            }

            Played = true;
            Confirmed = true;

        }
    }
}
