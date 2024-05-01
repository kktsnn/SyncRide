using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IUserService : IEntityService<App.BLL.DTO.Identity.AppUser>, IUserRepositoryShared<App.BLL.DTO.Identity.AppUser>
{
    Task<bool> AreUsersInChannel(Guid user1, Guid user2);
}