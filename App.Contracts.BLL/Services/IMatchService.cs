using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMatchService : IEntityService<App.BLL.DTO.Match>, IMatchRepositoryShared<App.BLL.DTO.Match>
{
    Task<IEnumerable<App.BLL.DTO.Match>> GetAllVisibleAsync(Guid userId);
    App.BLL.DTO.Match CalculateMatch(App.BLL.DTO.Route route1, App.BLL.DTO.Route route2);
    Task<App.BLL.DTO.Match> Accept(Guid id, Guid userId);
    Task<Match?> FindMatchBetweenUsersAsync(Guid id, Guid otherId);
}