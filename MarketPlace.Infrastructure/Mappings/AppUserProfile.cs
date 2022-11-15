using AutoMapper;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Identity;

namespace MarketPlace.Infrastructure.Mappings;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<User, AppUser>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email))
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Phone));
    }
}