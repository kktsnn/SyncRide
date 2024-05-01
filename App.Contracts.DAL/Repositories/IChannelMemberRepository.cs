using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IChannelMemberRepository : IEntityRepository<DalDto.ChannelMember>
{
}