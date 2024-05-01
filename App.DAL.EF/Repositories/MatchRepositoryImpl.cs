using App.Contracts.DAL.Repositories;
using AutoMapper;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MatchRepositoryImpl : BaseEntityRepository<AppDomain.Match, DalDto.Match, AppDbContext>, IMatchRepository
{
    public MatchRepositoryImpl(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Match, DalDto.Match>(mapper))
    {
    }

    protected override IQueryable<AppDomain.Match> IncludedQuery(Guid userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking)
            .Include(e => e.Route1)
            .Include(e => e.Route2);
    }

    public async Task<int> DeleteAllByRouteAsync(Guid routeId)
    {
        return await base.CreateQuery()
            .Where(m => m.Route1Id == routeId || m.Route2Id == routeId)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<DalDto.Match>> GetAllByUserIdAsync(Guid userId)
    {
        return (await base.CreateQuery()
            .Include(m => m.Route1)
            .Include(m => m.Route2)
            .Where(m => m.Route1!.AppUserId == userId || m.Route2!.AppUserId == userId)
            .ToListAsync()).Select(e => Mapper.Map(e)!);
    }
}