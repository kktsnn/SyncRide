using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using DalDto = App.DAL.DTO;
using BllDto = App.BLL.DTO;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class MessageHeaderServiceImpl : BaseEntityService<DalDto.MessageHeader, BllDto.MessageHeader, IMessageHeaderRepository>, IMessageHeaderService
{
    public MessageHeaderServiceImpl(IMessageHeaderRepository repository, IMapper mapper) : 
        base(repository, new BllDalMapper<DalDto.MessageHeader, BllDto.MessageHeader>(mapper))
    {
    }
}