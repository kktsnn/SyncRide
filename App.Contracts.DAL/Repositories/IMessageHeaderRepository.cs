using DalDto = App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IMessageHeaderRepository : IEntityRepository<DalDto.MessageHeader>
{
}