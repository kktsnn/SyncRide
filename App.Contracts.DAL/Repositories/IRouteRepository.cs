using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IRouteRepository : IEntityRepository<DalDto.Route>, IRouteRepositoryShared<DalDto.Route>
{
}

public interface IRouteRepositoryShared<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllNotOwnedByUserAsync(Guid userId);
}