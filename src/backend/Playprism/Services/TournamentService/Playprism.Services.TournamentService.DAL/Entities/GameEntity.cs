using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playprism.Services.TournamentService.DAL.Entities
{
    public class GameEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }
        public string Description { get; set; } 

        public virtual MatchEntity Match { get; set; }
    }
}
