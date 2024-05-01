using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Base.Domain;

namespace App.BLL.DTO;

public class Location : BaseEntityId
{
    [Required]
    [Display(ResourceType = typeof(Resources.Domain.RLocation), Name = nameof(Type))]
    public ELocationType Type { get; set; }
    
    [Required]
    [Display(ResourceType = typeof(Resources.Domain.RLocation), Name = nameof(Longitude))]
    public double Longitude { get; set; }
    
    [Required]
    [Display(ResourceType = typeof(Resources.Domain.RLocation), Name = nameof(Latitude))]
    public double Latitude { get; set; }
    
    [Required]
    [Display(ResourceType = typeof(Resources.Domain.RLocation), Name = nameof(Time))]
    public TimeOnly Time { get; set; }

    public override string ToString()
    {
        return $"{Type} {Longitude}:{Latitude} {Time}";
    }
}