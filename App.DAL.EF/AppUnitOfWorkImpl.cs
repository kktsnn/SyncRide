using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUnitOfWorkImpl : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    public AppUnitOfWorkImpl(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
    {
        _mapper = mapper;
    }
    
    private IChannelMemberRepository? _channelMembers;
    public IChannelMemberRepository ChannelMembers => 
        _channelMembers ??= new ChannelMemberRepositoryImpl(UowDbContext, _mapper);

    private IChannelRepository? _channels;
    public IChannelRepository Channels => 
        _channels ??= new ChannelRepositoryImpl(UowDbContext, _mapper);

    private ILocationRepository? _locations;
    public ILocationRepository Locations => 
        _locations ??= new LocationRepositoryImpl(UowDbContext, _mapper);

    private IMatchRepository? _matches;
    public IMatchRepository Matches => 
        _matches ??= new MatchRepositoryImpl(UowDbContext, _mapper);

    private IMessageHeaderRepository? _messageHeaders;
    public IMessageHeaderRepository MessageHeaders => 
        _messageHeaders ??= new MessageHeaderRepositoryImpl(UowDbContext, _mapper);

    private IMessageRepository? _messages;
    public IMessageRepository Messages => 
        _messages ??= new MessageRepositoryImpl(UowDbContext, _mapper);

    private IRecipientRepository? _recipients;
    public IRecipientRepository Recipients => 
        _recipients ??= new RecipientRepositoryImpl(UowDbContext, _mapper);
    
    private IRouteRepository? _routes;
    public IRouteRepository Routes => 
        _routes ??= new RouteRepositoryImpl(UowDbContext, _mapper);

    private IUserRepository? _users;

    public IUserRepository Users =>
        _users ??= new UserRepositoryImpl(UowDbContext, _mapper);
}