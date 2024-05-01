using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO.Identity;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class UserServiceImpl : BaseEntityService<App.DAL.DTO.Identity.AppUser, App.BLL.DTO.Identity.AppUser, IUserRepository>, IUserService
{
    public UserServiceImpl(IUserRepository repository, IMapper mapper) : 
        base(repository, new BllDalMapper<AppUser, DTO.Identity.AppUser>(mapper))
    {
    }

    public async Task<IEnumerable<App.BLL.DTO.Identity.AppUser>> GetAllMatchedUsersAsync(Guid id)
    {
        return (await Repository.GetAllMatchedUsersAsync(id)).Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> AreUsersInChannel(Guid user1, Guid user2)
    {
        return (await Repository.GetAllKnownUsersAsync(user1)).Any(u => u.Id == user2);
    }
}