using App.Contracts.DAL.Repositories;
using AutoMapper;
using AppDomain = App.Domain;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RecipientRepositoryImpl : BaseEntityRepository<AppDomain.Recipient, DalDto.Recipient, AppDbContext>, IRecipientRepository
{
    public RecipientRepositoryImpl(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.Recipient, DalDto.Recipient>(mapper))
    {
    }

    protected override IQueryable<AppDomain.Recipient> CreateQuery(Guid userId = default, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();

        if (userId != default)
        {
            query = query
                .Include(e => e.Member)
                .ThenInclude(cm => cm!.AppUser)
                .Where(r => r.Member!.AppUserId == userId);
        }

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    protected override IQueryable<AppDomain.Recipient> IncludedQuery(Guid userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking)
            .Include(e => e.MessageHeader)
            .ThenInclude(mh => mh!.Message)
            .Include(e => e.MessageHeader)
            .ThenInclude(mh => mh!.Sender)
            .ThenInclude(s => s!.AppUser);
    }

    public async Task<IEnumerable<DalDto.Recipient>> GetAllByChannelAsync(Guid channelId, Guid userId)
    {
        return (await IncludedQuery(userId)
            .Where(r => r.MessageHeader!.ChannelId == channelId)
            .ToListAsync())
            .Select(r => Mapper.Map(r)!);
    }
}