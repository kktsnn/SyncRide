using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using DalDto = App.DAL.DTO;
using BllDto = App.BLL.DTO;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class MatchServiceImpl : BaseEntityService<DalDto.Match, BllDto.Match, IMatchRepository>, IMatchService
{
    public MatchServiceImpl(IMatchRepository repository, IMapper mapper) : 
        base(repository, new BllDalMapper<DalDto.Match, BllDto.Match>(mapper))
    {
    }

    public async Task<int> DeleteAllByRouteAsync(Guid routeId)
    {
        return await Repository.DeleteAllByRouteAsync(routeId);
    }

    public async Task<IEnumerable<BllDto.Match>> GetAllVisibleAsync(Guid userId)
    {
        var matches = await Repository.GetAllByUserIdAsync(userId);
        return matches.Select(e => Mapper.Map(e)!).Where(
            m => !m.Accepted && (
                m.Route1!.AppUserId == userId 
                    ? !m.Route1Accepted ?? true
                    : !m.Route2Accepted ?? true)
            );
    }

    // TODO: Better match calculation
    public BllDto.Match CalculateMatch(BllDto.Route route1, BllDto.Route route2)
    {
        var dist = .0;
        foreach (var (l1, l2) in route1.Locations!.Zip(route2.Locations!))
        {
            dist += Math.Abs(l1.Latitude - l2.Latitude) + Math.Abs(l1.Longitude - l2.Longitude);
        }
        
        //TODO take time into account
        
        var match = new BllDto.Match
        {
            Route1Id = route1.Id,
            Route2Id = route2.Id,
            DeltaDist = dist,
            DeltaTime = dist * 60,
            MatchPercent = Math.Round(10 / (10 + dist) * 100, 2)
        };

        return match;
    }

    public async Task<App.BLL.DTO.Match> Accept(Guid id, Guid userId)
    {
        var m = (await FirstOrDefaultAsync(id))!;

        if (m.Route1!.AppUserId == userId)
        {
            m.Route1Accepted = true;
        }
        else
        {
            m.Route2Accepted = true;
        }

        return Update(m);
    }

    public async Task<BllDto.Match?> FindMatchBetweenUsersAsync(Guid id, Guid otherId)
    {
        return Mapper.Map((await Repository.GetAllByUserIdAsync(id)).FirstOrDefault(
            m => m.Route1!.AppUserId == id && m.Route2!.AppUserId == otherId ||
                 m.Route2!.AppUserId == id && m.Route1!.AppUserId == otherId));
    }
}