using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class Route : BaseEntityId
{
    [Required]
    public Guid AppUserId { get; set; }
    
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
    [MinLength(2)]
    [Display(ResourceType = typeof(Resources.Domain.RLocation), Name = nameof(Locations))]
    public ICollection<Location>? Locations { get; set; }
    
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
}
