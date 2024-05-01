using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace Base.BLL;

public abstract class BaseBll<TAppDbContext> : IBll
    where TAppDbContext : DbContext
{
    protected readonly IUnitOfWork Uow;

    protected BaseBll(IUnitOfWork uow)
    {
        Uow = uow;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }
}