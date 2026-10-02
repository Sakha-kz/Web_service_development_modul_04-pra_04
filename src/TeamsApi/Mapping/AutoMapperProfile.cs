using AutoMapper;
using TeamsApi.Models;

namespace TeamsApi.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Team, TeamDto>().ReverseMap();
    }
}
