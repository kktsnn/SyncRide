using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using DalDto = App.DAL.DTO;
using BllDto = App.BLL.DTO;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class RouteServiceImpl : BaseEntityService<DalDto.Route, BllDto.Route, IRouteRepository>, IRouteService
{
    public RouteServiceImpl(IRouteRepository repository, IMapper mapper) : 
        base(repository, new BllDalMapper<DalDto.Route, BllDto.Route>(mapper))
    {
    }

    public bool IsRouteOwnedByUser(Guid routeId, Guid userId)
    {
        return Repository.FirstOrDefault(routeId)?.AppUserId == userId;
    }

    public BllDto.Route AddToUser(Guid userId, BllDto.Route route)
    {
        var dto = Mapper.Map(route)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Add(dto))!;
    }

    public BllDto.Route UpdateForUser(Guid userId, BllDto.Route route)
    {
        var dto = Mapper.Map(route)!;
        dto.AppUserId = userId;
        return Mapper.Map(Repository.Update(dto))!;
    }

    public async Task<IEnumerable<BllDto.Route>> GetAllNotOwnedByUserAsync(Guid userId)
    {
        return (await Repository.GetAllNotOwnedByUserAsync(userId)).Select(e => Mapper.Map(e))!;
    }
}