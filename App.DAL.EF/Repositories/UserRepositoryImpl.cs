using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using App.Domain.Identity;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Match = App.Domain.Match;
using Route = App.Domain.Route;

namespace App.DAL.EF.Repositories;

public class UserRepositoryImpl : BaseEntityRepository<App.Domain.Identity.AppUser, DAL.DTO.Identity.AppUser, AppDbContext>, IUserRepository
{
    public UserRepositoryImpl(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppUser, DTO.Identity.AppUser>(mapper))
    {
    }
    
    public async Task<IEnumerable<DTO.Identity.AppUser>> GetAllMatchedUsersAsync(Guid id)
    {
        return (await CreateQuery()
            .Include(u => u.Routes)!
            .ThenInclude(r => r.Route1Matches)!
            .ThenInclude(m => m.Route2)
            .Include(u => u.Routes)!
            .ThenInclude(r => r.Route2Matches)!
            .ThenInclude(m => m.Route1)
            .ToListAsync()).Select(e => Mapper.Map(e)!)
            .Where(u => u.Id != id && u.Routes!.Any(
                r => r.Matches!.Any(m => 
                    m.Route1!.AppUserId == id || m.Route2!.AppUserId == id)
                )
            );
    }

    public async Task<IEnumerable<DTO.Identity.AppUser>> GetAllKnownUsersAsync(Guid userId)
    {
        return (await CreateQuery(default, false)
            .Include(u => u.Channels)!
            .ThenInclude(cm => cm.Channel)
            .ThenInclude(c => c!.Members)
            .Where(u => u.Id != userId && u.Channels!
                .Any(cm => cm.Channel!.Members!
                    .Any(m => m.AppUserId == userId)))
            .ToListAsync()).Select(u => Mapper.Map(u)!);
    }
}