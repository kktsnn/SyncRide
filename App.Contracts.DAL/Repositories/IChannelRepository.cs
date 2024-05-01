using App.Domain.Enums;
using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IChannelRepository : IEntityRepository<DalDto.Channel>
{
    Task<IEnumerable<DalDto.Channel>> GetAllByTypeAsync(EChannelType type);
}