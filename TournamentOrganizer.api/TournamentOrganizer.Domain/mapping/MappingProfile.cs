using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TournamentOrganizer.Domain.DTOs;
using TournamentOrganizer.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TournamentOrganizer.Domain.mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tournament, TournamentDto>()
                .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.Format.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(
                    dest => dest.CurrentParticipants,
                    opt => opt.MapFrom(src => src.Participants.Count)
                );

            CreateMap<CreateTournamentDto, Tournament>();
            CreateMap<TournamentParticipant, ParticipantDto>();
            CreateMap<RegisterParticipantDto, TournamentParticipant>();
            CreateMap<Match, MatchDto>();
        }
    }
}
