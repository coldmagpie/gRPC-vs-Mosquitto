using AutoMapper;
using DataAccess.Models;
using Domain.Dtos;

namespace API.Mappers;
public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>()
            .ForMember(m => m.FirstName, act => act.MapFrom(d => d.FirstName))
            .ForMember(m => m.LastName, act => act.MapFrom(d => d.LastName))
            .ForMember(m => m.NickName, act => act.MapFrom(d => d.NickName))
            .ForMember(m => m.Password, act => act.MapFrom(d => d.Password))
            .ReverseMap();
    }
}
