using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Match : BaseEntityId
{
    public Guid Route1Id { get; set; }
    
    public Guid Route2Id { get; set; }
    
    [Range(0, 100)]
    public double MatchPercent { get; set; }
    
    public double DeltaTime { get; set; }
    
    public double DeltaDist { get; set; }
    
    public bool? Route1Accepted { get; set; }
    
    public bool? Route2Accepted { get; set; }
    
    
    //Nav
    public Route? Route1 { get; set; }
    
    public Route? Route2 { get; set; }
}