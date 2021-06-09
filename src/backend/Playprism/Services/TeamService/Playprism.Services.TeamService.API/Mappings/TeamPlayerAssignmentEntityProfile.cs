using AutoMapper;
using Playprism.Services.TeamService.API.Dtos;
using Playprism.Services.TeamService.API.Entities;

namespace Playprism.Services.TeamService.API.Mappings
{
    public class TeamPlayerAssignmentEntityProfile: Profile
    {
        public TeamPlayerAssignmentEntityProfile()
        {
            CreateMap<TeamPlayerAssignmentEntity, AssignmentResponse>()
                .ForMember(x => x.Team, opt => opt.MapFrom(x => x.Team))
                .ForMember(x => x.IsOwner, opt => opt.Ignore());
        }
    }
}
