using Playprism.Services.TournamentService.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playprism.Services.TournamentService.DAL.Entities
{
    public class TournamentEntity : Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Discipline")]
        public int DisciplineId { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        public bool AreTeams { get; set; }
        [Required]
        public int MaxNumberOfPlayers { get; set; }
        [Required]
        public string Timezone { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string LogoPath { get; set; }
        public string Description { get; set; }
        public string Prizes { get; set; }
        public string Rules { get; set; }
        public string RulesPath { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        [Required]
        public TournamentFormatEnum Format { get; set; }
        public bool InviteOnly { get; set; }
        public bool RegistrationApprovalNeeded { get; set; }
        public int? MinNumberOfPlayersInTeam { get; set; }
        public int? MaxNumberOfPlayersInTeam { get; set; }
        public bool Published { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Finished { get; set; }
        public bool Aborted { get; set; }

        public virtual DisciplineEntity Discipline { get; set; }
        public ICollection<ParticipantEntity> Participants { get; set; }
    }
}
