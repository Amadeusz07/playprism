using System;

namespace Playprism.Services.TeamService.API.Dtos
{
    public class AssignmentResponse
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public DateTime InviteDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public bool Accepted { get; set; }
        public bool Active { get; set; }
        public TeamResponse Team { get; set; }
    }

    public class TeamResponse
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public string Contact { get; set; }
        public string Country { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Active { get; set; }
    }
}
