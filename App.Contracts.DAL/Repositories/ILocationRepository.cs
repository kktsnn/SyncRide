using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ILocationRepository : IEntityRepository<DalDto.Location>, ILocationRepositoryShared<DalDto.Location>
{
}

public interface ILocationRepositoryShared<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllByRouteIdAsync(Guid id);
}