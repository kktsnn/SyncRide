using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Domain;

namespace App.DAL.DTO;

public class Match : BaseEntityId
{
    public Guid Route1Id { get; set; }
    
    public Guid Route2Id { get; set; }
    
    [Range(0, 100)]
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = nameof(MatchPercent))]
    public double MatchPercent { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = nameof(DeltaTime))]
    public double DeltaTime { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = nameof(DeltaDist))]
    public double DeltaDist { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = "Accepted")]
    public bool? Route1Accepted { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMatch), Name = "Accepted")]
    public bool? Route2Accepted { get; set; }
    
    
    //Nav
    [Display(ResourceType = typeof(Resources.Domain.RRoute), Name = nameof(Route))]
    public Route? Route1 { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RRoute), Name = nameof(Route))]
    public Route? Route2 { get; set; }
}