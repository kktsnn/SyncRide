using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DAL.DTO.ChannelMember, Domain.ChannelMember>().ReverseMap();
        CreateMap<DAL.DTO.Channel, Domain.Channel>().ReverseMap();
        CreateMap<DAL.DTO.Location, Domain.Location>().ReverseMap();
        CreateMap<DAL.DTO.Match, Domain.Match>().ReverseMap();
        CreateMap<DAL.DTO.Message, Domain.Message>().ReverseMap();
        CreateMap<DAL.DTO.MessageHeader, Domain.MessageHeader>().ReverseMap();
        CreateMap<DAL.DTO.Recipient, Domain.Recipient>().ReverseMap();
        CreateMap<DAL.DTO.Route, Domain.Route>().ReverseMap();
        
        CreateMap<DAL.DTO.Identity.AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
}