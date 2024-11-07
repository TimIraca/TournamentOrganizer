using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tournament, TournamentCoreDto>()
                .ForMember(
                    dest => dest.CurrentParticipants,
                    opt => opt.MapFrom(src => src.Participants.Count)
                )
                .ReverseMap();
            CreateMap<TournamentParticipant, TournamentParticipantCoreDto>().ReverseMap();
        }
    }
}
