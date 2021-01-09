using System.ComponentModel.DataAnnotations;

namespace Playprism.Services.TournamentService.DAL.Entities
{
    public class DisciplineEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string LogoPath { get; set; }
    }
}
