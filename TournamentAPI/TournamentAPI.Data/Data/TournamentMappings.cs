using TournamentAPI.Core.Dto;
using TournamentAPI.Core.Entities;
using AutoMapper;

namespace TournamentAPI.Data.Data;

public class TournamentMappings : Profile
{
    public TournamentMappings()
    {
        CreateMap<Tournament, TournamentDto>();
        CreateMap<Game, GameDto>();
    }
}
