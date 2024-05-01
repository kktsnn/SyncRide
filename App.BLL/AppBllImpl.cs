using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.DAL.DTO.Identity;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.BLL;

public class AppBllImpl : BaseBll<AppDbContext>, IAppBll
{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    
    public AppBllImpl(IAppUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _uow = uow;
        _mapper = mapper;
    }

    private IChannelMemberService? _channelMembers;
    public IChannelMemberService ChannelMembers => 
        _channelMembers ??= new ChannelMemberServiceImpl(_uow.ChannelMembers, _mapper);

    private IChannelService? _channels;
    public IChannelService Channels => 
        _channels ??= new ChannelServiceImpl(_uow.Channels, _mapper);

    private ILocationService? _locations;
    public ILocationService Locations => 
        _locations ??= new LocationServiceImpl(_uow.Locations, _mapper);

    private IMatchService? _matches;
    public IMatchService Matches => 
        _matches ??= new MatchServiceImpl(_uow.Matches, _mapper);

    private IMessageHeaderService? _messageHeaders;
    public IMessageHeaderService MessageHeaders => 
        _messageHeaders ??= new MessageHeaderServiceImpl(_uow.MessageHeaders, _mapper);

    private IMessageService? _messages;
    public IMessageService Messages => 
        _messages ??= new MessageServiceImpl(_uow.Messages, _mapper);

    private IRecipientService? _recipients;
    public IRecipientService Recipients => 
        _recipients ??= new RecipientServiceImpl(_uow.Recipients, _mapper);
    
    private IRouteService? _routes;
    public IRouteService Routes => 
        _routes ??= new RouteServiceImpl(_uow.Routes, _mapper);

    private IUserService? _users;

    public IUserService Users =>
        _users ??= new UserServiceImpl(_uow.Users, _mapper);
}