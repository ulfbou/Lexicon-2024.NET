using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Entities;
using AutoMapper;

namespace TournamentAPI.Data.Data;

public class TournamentMappings : Profile
{
    public TournamentMappings()
    {
        CreateMap<Tournament, TournamentDto>();
        CreateMap<Tournament, TournamentDto>().ReverseMap();
        CreateMap<Game, GameDto>();
        CreateMap<Game, GameDto>().ReverseMap();
        CreateMap<Game, TournamentGameDto>();
        CreateMap<Game, TournamentGameDto>().ReverseMap();
    }
}
