using AutoMapper;
using Pokemon.Api.Response;

namespace Pokemon.Api.Mapper
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<Persistency.Pokemon, PokemonResponse>()
                .ForMember(x => x.Name, opt => opt.MapFrom(a => a.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(a => a.Description))
                .ForMember(x => x.HabitantType, opt => opt.MapFrom(a => a.HabitantType.ToString()))
                .ForMember(x => x.IsLegendary, opt => opt.MapFrom(a => a.IsLegendary));


        }
    }
}
