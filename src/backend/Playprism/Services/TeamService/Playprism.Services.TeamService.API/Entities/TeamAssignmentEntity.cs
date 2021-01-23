using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playprism.Services.TeamService.API.Entities
{
    public class TeamAssignmentEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public DateTime InviteDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public bool Accepted { get; set; }
        public bool Active { get; set; }

        public virtual PlayerEntity Player { get; set; }
        public virtual TeamEntity Team { get; set; }
    }
}
