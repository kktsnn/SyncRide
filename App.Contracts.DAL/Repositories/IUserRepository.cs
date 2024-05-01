using App.DAL.DTO.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IUserRepository : IEntityRepository<App.DAL.DTO.Identity.AppUser>, IUserRepositoryShared<App.DAL.DTO.Identity.AppUser>
{
    Task<IEnumerable<AppUser>> GetAllKnownUsersAsync(Guid userId);
}

public interface IUserRepositoryShared<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllMatchedUsersAsync(Guid id);
}