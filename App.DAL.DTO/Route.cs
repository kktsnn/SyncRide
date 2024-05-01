using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Contracts.Domain;
using Base.Domain;
using AppUser = App.DAL.DTO.Identity.AppUser;

namespace App.DAL.DTO;

public class Route : BaseEntityId, IDomainAppUser<AppUser>
{
    [Required]
    public Guid AppUserId { get; set; }
    
    public Guid? ChannelId { get; set; }
    
    [Required]
    [MaxLength(64)]
    [Display(ResourceType = typeof(Resources.Domain.RRoute), Name = nameof(Name))]
    public string Name { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.Domain.RRoute), Name = nameof(Active))]
    public bool Active { get; set; } = true;

    [MaxLength(1024)]
    [Display(ResourceType = typeof(Resources.Domain.RRoute), Name = nameof(Comment))]
    public string? Comment { get; set; }
    
    
    // Nav
    public ICollection<DAL.DTO.Location>? Locations { get; set; }
    
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Channel))]
    public DAL.DTO.Channel? Channel { get; set; }
    
    // Match logic
    [InverseProperty("Route1")] 
    public ICollection<DAL.DTO.Match>? Route1Matches { get; set; }
    
    [InverseProperty("Route2")] 
    public ICollection<DAL.DTO.Match>? Route2Matches { get; set; }

    [NotMapped] 
    public ICollection<DAL.DTO.Match>? Matches =>
        Route1Matches == null ? Route2Matches :
        Route2Matches == null ? Route1Matches : 
        Route1Matches.Concat(Route2Matches).ToList();
}
