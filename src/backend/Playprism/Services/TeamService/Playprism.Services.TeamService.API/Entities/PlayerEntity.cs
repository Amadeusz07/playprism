using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playprism.Services.TeamService.API.Entities
{
    public class PlayerEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Name { get; set; }

        public ICollection<TeamPlayerAssignmentEntity> Assignments { get; set; }
    }
}
