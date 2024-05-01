using AutoMapper;

namespace App.BLL;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<DAL.DTO.ChannelMember, BLL.DTO.ChannelMember>().ReverseMap();
        CreateMap<DAL.DTO.Channel, BLL.DTO.Channel>().ReverseMap();
        CreateMap<DAL.DTO.Location, BLL.DTO.Location>().ReverseMap();
        CreateMap<DAL.DTO.Match, BLL.DTO.Match>().ReverseMap();
        CreateMap<DAL.DTO.Message, BLL.DTO.Message>().ReverseMap();
        CreateMap<DAL.DTO.MessageHeader, BLL.DTO.MessageHeader>().ReverseMap();
        CreateMap<DAL.DTO.Recipient, BLL.DTO.Recipient>().ReverseMap();
        CreateMap<DAL.DTO.Route, BLL.DTO.Route>().ReverseMap();
        
        CreateMap<DAL.DTO.Identity.AppUser, BLL.DTO.Identity.AppUser>().ReverseMap();
    }
}