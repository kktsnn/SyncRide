using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using App.Domain.Enums;
using AutoMapper;
using DalDto = App.DAL.DTO;
using BllDto = App.BLL.DTO;
using Base.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class ChannelServiceImpl : BaseEntityService<DalDto.Channel, BllDto.Channel, IChannelRepository>, IChannelService
{
    public ChannelServiceImpl(IChannelRepository repository, IMapper mapper) : 
        base(repository, new BllDalMapper<DalDto.Channel, BllDto.Channel>(mapper))
    {
    }

    public async Task<BllDto.Channel?> DmExistsBetweenUsers(Guid user1Id, Guid user2Id)
    {
        return Mapper.Map((await Repository.GetAllByTypeAsync(EChannelType.Direct)).FirstOrDefault(c =>
            c.Members!.Any(m => m.AppUserId == user1Id) && c.Members!.Any(m => m.AppUserId == user2Id)));
    }
}