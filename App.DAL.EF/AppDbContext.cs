using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DbSet<Channel> Channels { get; set; } = default!;
    public DbSet<ChannelMember> ChannelMembers { get; set; } = default!;
    public DbSet<Location> Locations { get; set; } = default!;
    public DbSet<Match> Matches { get; set; } = default!;
    public DbSet<MessageHeader> MessageHeaders { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<Recipient> Recipients { get; set; } = default!;
    public DbSet<Route> Routes { get; set; } = default!;

    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entity in ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted))
        {
            foreach (var prop in entity.Properties.Where(x => x.Metadata.ClrType == typeof(DateTime)))
            {
                prop.CurrentValue = ((DateTime)prop.CurrentValue!).ToUniversalTime();
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}