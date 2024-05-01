using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IRecipientRepository : IEntityRepository<DalDto.Recipient>, IRecipientRepositoryShared<DalDto.Recipient>
{
}

public interface IRecipientRepositoryShared<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllByChannelAsync(Guid channelId, Guid userId);
}