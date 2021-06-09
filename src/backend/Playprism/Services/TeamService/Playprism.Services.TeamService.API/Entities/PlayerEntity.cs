using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playprism.Services.TeamService.API.Entities
{
    public class PlayerEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public ICollection<TeamPlayerAssignmentEntity> Assignments { get; set; }
    }
}
