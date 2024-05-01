using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Channel : BaseEntityId
{
    public EChannelType Type { get; set; }
    
    [MaxLength(64)]
    public string? Name { get; set; }
    
    [MaxLength(1024)]
    public string? Description { get; set; }
    
    public Guid? OwnerId { get; set; }
    
    // Nav
    public AppUser? Owner { get; set; }
    
    public ICollection<ChannelMember>? Members { get; set; }
}