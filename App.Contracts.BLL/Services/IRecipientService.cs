using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IRecipientService : IEntityService<App.BLL.DTO.Recipient>, IRecipientRepositoryShared<App.BLL.DTO.Recipient>
{
}