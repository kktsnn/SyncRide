using App.Contracts.DAL.Repositories;
using AutoMapper;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RouteRepositoryImpl : BaseEntityRepository<AppDomain.Route, DalDto.Route, AppDbContext>, IRouteRepository
{
    public RouteRepositoryImpl(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Route, DalDto.Route>(mapper))
    {
    }

    protected override IQueryable<AppDomain.Route> IncludedQuery(Guid userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking)
            .Include(e => e.AppUser)
            .Include(e => e.Channel)
            .Include(e => e.Locations);
    }

    public async Task<IEnumerable<DalDto.Route>> GetAllNotOwnedByUserAsync(Guid userId)
    {
        return (await base.CreateQuery()
            .Include(r => r.Locations)
            .Where(r => r.AppUserId != userId)
            .ToListAsync())
            .Select(e => Mapper.Map(e))!;
    }
}