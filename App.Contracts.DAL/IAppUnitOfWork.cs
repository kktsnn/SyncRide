using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    // list repos here
    IChannelMemberRepository ChannelMembers { get; }
    IChannelRepository Channels { get; }
    ILocationRepository Locations { get; }
    IMatchRepository Matches { get; }
    IMessageHeaderRepository MessageHeaders { get; }
    IMessageRepository Messages { get; }
    IRecipientRepository Recipients { get; }
    IRouteRepository Routes { get; }
    IUserRepository Users { get; }
}