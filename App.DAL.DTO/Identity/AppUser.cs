using App.Domain;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.DAL.DTO.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string? About { get; set; }
    
    public string? ProfilePictureExtension { get; set; }

    
    //Navigational
    public ICollection<Route>? Routes { get; set; }
    
    public ICollection<ChannelMember>? Channels { get; set; }
    
    public ICollection<Channel>? OwnedChannels { get; set; }
    
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
}