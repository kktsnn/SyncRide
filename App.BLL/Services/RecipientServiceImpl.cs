using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using DalDto = App.DAL.DTO;
using BllDto = App.BLL.DTO;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class RecipientServiceImpl : BaseEntityService<DalDto.Recipient, BllDto.Recipient, IRecipientRepository>, IRecipientService
{
    public RecipientServiceImpl(IRecipientRepository repository, IMapper mapper) : 
        base(repository, new BllDalMapper<DalDto.Recipient, BllDto.Recipient>(mapper))
    {
    }

    public async Task<IEnumerable<BllDto.Recipient>> GetAllByChannelAsync(Guid channelId, Guid userId)
    {
        return (await Repository.GetAllByChannelAsync(channelId, userId)).Select(r => Mapper.Map(r)!);
    }
}