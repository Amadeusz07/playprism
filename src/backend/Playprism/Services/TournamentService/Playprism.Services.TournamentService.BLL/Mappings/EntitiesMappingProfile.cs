using AutoMapper;
using Playprism.Services.TournamentService.BLL.Dtos;
using Playprism.Services.TournamentService.DAL.Entities;

namespace Playprism.Services.TournamentService.BLL.Mappings
{
    public class EntitiesMappingProfile: Profile
    {
        public EntitiesMappingProfile()
        {
            CreateMap<TournamentEntity, TournamentEntity>()
                .ForMember(x => x.Id, o => o.Ignore());
            CreateMap<TournamentEntity, TournamentListItemResponse>()
                .ForMember(x => x.CurrentNumberOfPlayers, opt => opt.MapFrom(x => x.Participants.Count))
                .ForMember(x => x.DisciplineName, opt => opt.MapFrom(x => x.Discipline.Name));
            CreateMap<TournamentEntity, TournamentDetailsResponse>()
                .ForMember(x => x.CurrentNumberOfPlayers, opt => opt.MapFrom(x => x.Participants.Count))
                .ForMember(x => x.DisciplineName, opt => opt.MapFrom(x => x.Discipline.Name));
            CreateMap<CreateTournamentRequest, TournamentEntity>();
            CreateMap<UpdateTournamentRequest, TournamentEntity>()
                .ForMember(x => x.ContactNumber, opt => opt.MapFrom(x => x.ContactNumber.ToString()));

            CreateMap<MatchDefinitionEntity, MatchDefinitionEntity>()
                .ForMember(x => x.Id, o => o.Ignore());
            CreateMap<UpdateMatchDefinitionRequest, MatchDefinitionEntity>();

            CreateMap<MatchEntity, MatchEntity>()
                .ForMember(x => x.Id, o => o.Ignore());
            CreateMap<MatchEntity, MatchDto>()
                .ForMember(x => x.MatchDefinition, opt => opt.MapFrom(x => x.Round.MatchDefinition))
                .ForMember(x => x.TournamentName, opt => opt.MapFrom(x => x.Tournament.Name))
                .ForMember(x => x.Participant1Name, opt => opt.Ignore())
                .ForMember(x => x.Participant2Name, opt => opt.Ignore());
        }
    }
}