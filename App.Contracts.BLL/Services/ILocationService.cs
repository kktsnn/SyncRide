using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ILocationService : IEntityService<App.BLL.DTO.Location>, ILocationRepositoryShared<App.BLL.DTO.Location>
{
    App.BLL.DTO.Location AddToRoute(Guid routeId, App.BLL.DTO.Location location);
    App.BLL.DTO.Location UpdateWithRoute(Guid routeId, App.BLL.DTO.Location location);
}