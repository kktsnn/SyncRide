using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain.Identity;

public class AppRefreshToken : BaseRefreshToken, IDomainAppUserId
{
    public Guid AppUserId { get; set; }
    
    // Nav
    public AppUser? AppUser { get; set; }
}