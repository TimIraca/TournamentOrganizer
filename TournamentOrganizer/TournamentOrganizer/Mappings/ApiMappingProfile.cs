using AutoMapper;
using TournamentOrganizer.api.DTOs.Requests;
using TournamentOrganizer.api.DTOs.Responses;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.api.Mappings
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<TournamentCoreDto, TournamentResponseApiDto>()
                .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.Format.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();
            CreateMap<TournamentCoreDto, TournamentRequestApiDto>().ReverseMap();
            CreateMap<TournamentCoreDto, CreateTournamentApiDto>().ReverseMap();
            CreateMap<TournamentParticipantRequestApiDto, TournamentParticipantCoreDto>();
            CreateMap<TournamentParticipantCoreDto, TournamentParticipantResponseApiDto>();
        }
    }
}
