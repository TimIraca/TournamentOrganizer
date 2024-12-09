using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.DTOs.Overview;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tournament, TournamentCoreDto>().ReverseMap();
            CreateMap<Participant, ParticipantCoreDto>().ReverseMap();
            CreateMap<Round, RoundCoreDto>().ReverseMap();
            CreateMap<Match, MatchCoreDto>().ReverseMap();
            CreateMap<Participant, ParticipantOverviewDto>().ReverseMap();
            CreateMap<Match, MatchOverviewDto>().ReverseMap();
            CreateMap<Round, RoundOverviewDto>().ReverseMap();
        }
    }
}
