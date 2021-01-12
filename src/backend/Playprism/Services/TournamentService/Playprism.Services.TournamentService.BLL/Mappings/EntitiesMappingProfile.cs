using AutoMapper;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.BLL.Mappings
{
    public class EntitiesMappingProfile: Profile
    {
        public EntitiesMappingProfile()
        {
            CreateMap<TournamentEntity, TournamentEntity>()
                .ForMember(x => x.Id, o => o.Ignore());
            CreateMap<MatchDefinitionEntity, MatchDefinitionEntity>()
                .ForMember(x => x.Id, o => o.Ignore());
            CreateMap<MatchEntity, MatchEntity>()
                .ForMember(x => x.Id, o => o.Ignore());
        }
    }
}