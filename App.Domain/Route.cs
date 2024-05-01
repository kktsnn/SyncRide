using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Route : BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid AppUserId { get; set; }
    
    public Guid? ChannelId { get; set; }
    
    [MaxLength(64)]
    public string Name { get; set; } = default!;
    
    public bool Active { get; set; } = true;

    [MaxLength(1024)]
    public string? Comment { get; set; }
    
    
    // Nav
    public ICollection<Location>? Locations { get; set; }
    
    public AppUser? AppUser { get; set; }
    
    public Channel? Channel { get; set; }
    
    // Match logic
    [InverseProperty("Route1")] 
    public ICollection<Match>? Route1Matches { get; set; }
    
    [InverseProperty("Route2")] 
    public ICollection<Match>? Route2Matches { get; set; }
}
