using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Playprism.Services.TeamService.API.Entities
{
    public class TeamEntity: Entity
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public string Contact { get; set; }
        public string Country { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }

        public ICollection<PlayerEntity> Players { get; set; }
    }
}
