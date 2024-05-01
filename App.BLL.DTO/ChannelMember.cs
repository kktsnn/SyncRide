using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Base.Domain;
using AppUser = App.BLL.DTO.Identity.AppUser;

namespace App.BLL.DTO;

public class ChannelMember : BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid ChannelId { get; set; }
    
    public Guid AppUserId { get; set; }
    
    [MaxLength(64)]
    [Display(ResourceType = typeof(Resources.Domain.RChannelMember), Name = nameof(Nickname))]
    public string? Nickname { get; set; }

    public string Name => Nickname ?? $"{AppUser?.FirstName} {AppUser?.LastName}";
    
    
    //Nav
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Channel))]
    public Channel? Channel { get; set; }
    
    public ICollection<MessageHeader>? SentMessages { get; set; }
}