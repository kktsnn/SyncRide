using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IRouteService : IEntityService<App.BLL.DTO.Route>, IRouteRepositoryShared<App.BLL.DTO.Route>
{
    bool IsRouteOwnedByUser(Guid routeId, Guid userId);
    App.BLL.DTO.Route AddToUser(Guid userId, App.BLL.DTO.Route route);
    App.BLL.DTO.Route UpdateForUser(Guid userId, App.BLL.DTO.Route route);
}