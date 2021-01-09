using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playprism.Services.TournamentService.DAL.Entities
{
    public class MatchDefinitionEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool ConfirmationNeeded { get; set; }
        [Required]
        public int NumberOfGames { get; set; }
        [Required]
        public bool ScoreBased { get; set; }
    }
    
}
