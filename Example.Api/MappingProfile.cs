using AutoMapper;

namespace Example.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Db.Entities.User, Models.RegisterRequest>().ReverseMap();
            CreateMap<Db.Entities.User, Models.RegisterResponse>().ReverseMap();
            CreateMap<Db.Entities.User, Models.User>().ReverseMap();
        }
    }
}
