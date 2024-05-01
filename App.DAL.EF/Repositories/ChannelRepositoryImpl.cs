using App.Contracts.DAL.Repositories;
using App.Domain.Enums;
using AutoMapper;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ChannelRepositoryImpl : BaseEntityRepository<AppDomain.Channel, DalDto.Channel, AppDbContext>, IChannelRepository
{
    public ChannelRepositoryImpl(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Channel, DalDto.Channel>(mapper))
    {
    }

    protected override IQueryable<AppDomain.Channel> CreateQuery(Guid userId = default, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();

        if (userId != default) query = query
            .Include(c => c.Members)!
            .ThenInclude(m => m.AppUser)
            .Where(c => c.Members!.Any(m => m.AppUserId == userId));

        if (noTracking) query = query.AsNoTracking();

        return query;
    }

    protected override IQueryable<AppDomain.Channel> IncludedQuery(Guid userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking)
            .Include(e => e.Owner);
    }

    public async Task<IEnumerable<DalDto.Channel>> GetAllByTypeAsync(EChannelType type)
    {
        return (await IncludedQuery().Where(c => c.Type == type).ToListAsync()).Select(e => Mapper.Map(e)!);
    }
}