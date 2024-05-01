using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using DalDto = App.DAL.DTO;
using BllDto = App.BLL.DTO;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class LocationServiceImpl : BaseEntityService<DalDto.Location, BllDto.Location, ILocationRepository>, ILocationService
{
    public LocationServiceImpl(ILocationRepository repository, IMapper mapper) : 
        base(repository, new BllDalMapper<DalDto.Location, BllDto.Location>(mapper))
    {
    }

    public BllDto.Location AddToRoute(Guid routeId, BllDto.Location location)
    {
        var e = Mapper.Map(location)!;
        
        e.RouteId = routeId;
        
        return Mapper.Map(Repository.Add(e))!;
    }

    public BllDto.Location UpdateWithRoute(Guid routeId, BllDto.Location location)
    {
        var e = Mapper.Map(location)!;
        
        e.RouteId = routeId;
        
        return Mapper.Map(Repository.Update(e))!;
    }

    public async Task<IEnumerable<BllDto.Location>> GetAllByRouteIdAsync(Guid id)
    {
        return (await Repository.GetAllByRouteIdAsync(id)).Select(e => Mapper.Map(e)!);
    }
}