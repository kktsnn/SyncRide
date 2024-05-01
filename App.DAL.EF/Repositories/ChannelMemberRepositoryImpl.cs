using App.Contracts.DAL.Repositories;
using AutoMapper;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ChannelMemberRepositoryImpl : BaseEntityRepository<AppDomain.ChannelMember, DalDto.ChannelMember, AppDbContext>, IChannelMemberRepository
{
    public ChannelMemberRepositoryImpl(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.ChannelMember, DalDto.ChannelMember>(mapper))
    {
    }

    protected override IQueryable<AppDomain.ChannelMember> IncludedQuery(Guid userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking)
            .Include(e => e.AppUser)
            .Include(e => e.Channel);
    }
}