using App.Contracts.DAL.Repositories;
using AutoMapper;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class LocationRepositoryImpl : BaseEntityRepository<AppDomain.Location, DalDto.Location, AppDbContext>, ILocationRepository
{
    public LocationRepositoryImpl(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Location, DalDto.Location>(mapper))
    {
    }

    protected override IQueryable<AppDomain.Location> IncludedQuery(Guid userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking)
            .Include(e => e.Route);
    }

    public async Task<IEnumerable<DalDto.Location>> GetAllByRouteIdAsync(Guid id)
    {
        return (await base.CreateQuery()
            .Where(l => l.RouteId == id)
            .ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }
}