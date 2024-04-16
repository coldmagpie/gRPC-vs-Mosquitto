using AutoMapper;
using DataAccess.Models;
using Domain.Dtos;
namespace API.Mapper;

public class MessageMapper : Profile
{
    public MessageMapper()
    {
        CreateMap<MessageModel, MessageDto>()
            .ForMember(m => m.Sender, act => act.MapFrom(d => d.Sender))
            .ForMember(m => m.Recipient, act => act.MapFrom(d => d.Recipient))
            .ForMember(m => m.Content, act => act.MapFrom(d => d.Content))
            .ForMember(m => m.TimeStamp, act => act.MapFrom(d => d.TimeStamp))
            .ReverseMap();
    }
    
}
