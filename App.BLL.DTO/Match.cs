using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class Match : BaseEntityId
{
    [Required]
    public Guid Route1Id { get; set; }
    
    [Required]
    public Guid Route2Id { get; set; }
    
    [Required]
    [Range(0, 100)]
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = nameof(MatchPercent))]
    public double MatchPercent { get; set; }
    
    [Required]
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = nameof(DeltaTime))]
    public double DeltaTime { get; set; }
    
    [Required]
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = nameof(DeltaDist))]
    public double DeltaDist { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = "Accepted")]
    public bool? Route1Accepted { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = "Accepted")]
    public bool? Route2Accepted { get; set; }

    public bool Accepted => (Route1Accepted ?? false) && (Route2Accepted ?? false);
    
    
    //Nav
    [Display(ResourceType = typeof(Resources.Domain.RRoute), Name = nameof(Route))]
    public Route? Route1 { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RRoute), Name = nameof(Route))]
    public Route? Route2 { get; set; }
}