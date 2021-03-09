using AutoMapper;
using Playprism.Services.TeamService.API.Dtos;
using Playprism.Services.TeamService.API.Entities;

namespace Playprism.Services.TeamService.API.Mappings
{
    public class TeamEntityProfile: Profile
    {
        public TeamEntityProfile()
        {
            CreateMap<TeamEntity, TeamResponse>();
        }
    }
}
