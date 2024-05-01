using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Contracts.Domain;
using Base.Domain;
using AppUser = App.DAL.DTO.Identity.AppUser;

namespace App.DAL.DTO;

public class ChannelMember : BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid ChannelId { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public Guid? RouteId { get; set; }
    
    [MaxLength(64)]
    [Display(ResourceType = typeof(Resources.Domain.RChannelMember), Name = nameof(Nickname))]
    public string? Nickname { get; set; }

    public string Name => Nickname ?? AppUser?.UserName ?? Id.ToString();
    
    
    //Nav
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Channel))]
    public DAL.DTO.Channel? Channel { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RRoute), Name = nameof(Route))]
    public Route? Route { get; set; }
    
    public ICollection<MessageHeader>? SentMessages { get; set; }
}