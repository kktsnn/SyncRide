using App.Contracts.DAL.Repositories;
using AutoMapper;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MessageRepositoryImpl : BaseEntityRepository<AppDomain.Message, DalDto.Message, AppDbContext>, IMessageRepository
{
    public MessageRepositoryImpl(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Message, DalDto.Message>(mapper))
    {
    }

    protected override IQueryable<AppDomain.Message> IncludedQuery(Guid userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking)
            .Include(e => e.Header);
    }
}