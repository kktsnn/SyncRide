using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IChannelService : IEntityService<App.BLL.DTO.Channel>
{
    Task<Channel?> DmExistsBetweenUsers(Guid user1Id, Guid user2Id);
}