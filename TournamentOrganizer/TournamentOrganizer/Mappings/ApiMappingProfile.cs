using AutoMapper;
using TournamentOrganizer.api.DTOs;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.api.Mappings
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<TournamentCoreDto, TournamentApiDto>().ReverseMap();
            CreateMap<ParticipantCoreDto, ParticipantApiDto>().ReverseMap();
            CreateMap<RoundCoreDto, RoundApiDto>().ReverseMap();
            CreateMap<MatchCoreDto, MatchApiDto>().ReverseMap();
            CreateMap<ParticipantCoreDto, CreateParticipantApiDto>().ReverseMap();
            CreateMap<TournamentCoreDto, CreateTournamentApiDto>().ReverseMap();
        }
    }
}
