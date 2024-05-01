using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Base.Domain;

namespace App.Domain;

public class Location : BaseEntityId
{
    public ELocationType Type { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public TimeOnly Time { get; set; }
    public Guid RouteId { get; set; }
    
    
    //Nav
    public Route? Route { get; set; }
}