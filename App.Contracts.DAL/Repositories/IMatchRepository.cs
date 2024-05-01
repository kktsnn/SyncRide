using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IMatchRepository : IEntityRepository<DalDto.Match>, IMatchRepositoryShared<DalDto.Match>
{
    Task<IEnumerable<DalDto.Match>> GetAllByUserIdAsync(Guid userId);
}

public interface IMatchRepositoryShared<TEntity>
{
    Task<int> DeleteAllByRouteAsync(Guid routeId);
}