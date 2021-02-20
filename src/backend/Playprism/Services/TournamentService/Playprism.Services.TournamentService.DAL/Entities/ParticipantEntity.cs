using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playprism.Services.TournamentService.DAL.Entities
{
    public class ParticipantEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool Approved { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual TournamentEntity Tournament { get; set; }
    }

}
