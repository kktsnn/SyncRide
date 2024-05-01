using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using DalDto = App.DAL.DTO;
using BllDto = App.BLL.DTO;
using Base.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class ChannelMemberServiceImpl : BaseEntityService<DalDto.ChannelMember, BllDto.ChannelMember, IChannelMemberRepository>, IChannelMemberService
{
    public ChannelMemberServiceImpl(IChannelMemberRepository repository, IMapper mapper) : 
        base(repository, new BllDalMapper<DalDto.ChannelMember, BllDto.ChannelMember>(mapper))
    {
    }
}