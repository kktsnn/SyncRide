using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class ChannelMember : BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid ChannelId { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public Guid? RouteId { get; set; }
    
    [MaxLength(64)]
    public string? Nickname { get; set; }
    
    
    //Nav
    public AppUser? AppUser { get; set; }
    
    public Channel? Channel { get; set; }
    
    
    public ICollection<MessageHeader>? SentMessages { get; set; }
}