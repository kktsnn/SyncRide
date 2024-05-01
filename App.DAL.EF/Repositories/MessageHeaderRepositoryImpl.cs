using App.Contracts.DAL.Repositories;
using AutoMapper;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MessageHeaderRepositoryImpl : BaseEntityRepository<AppDomain.MessageHeader, DalDto.MessageHeader, AppDbContext>, IMessageHeaderRepository
{
    public MessageHeaderRepositoryImpl(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.MessageHeader, DalDto.MessageHeader>(mapper))
    {
    }

    protected override IQueryable<AppDomain.MessageHeader> IncludedQuery(Guid userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking)
            .Include(e => e.Channel)
            .Include(e => e.Sender)
            .Include(e => e.Parent);
    }
}