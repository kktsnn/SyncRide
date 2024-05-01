using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBll : IBll
{
    // list of services here
    IChannelMemberService ChannelMembers { get; }
    IChannelService Channels { get; }
    ILocationService Locations { get; }
    IMatchService Matches { get; }
    IMessageHeaderService MessageHeaders { get; }
    IMessageService Messages { get; }
    IRecipientService Recipients { get; }
    IRouteService Routes { get; }
    IUserService Users { get; }
}