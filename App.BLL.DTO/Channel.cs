using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Base.Domain;
using AppUser = App.BLL.DTO.Identity.AppUser;

namespace App.BLL.DTO;

public class Channel : BaseEntityId
{
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Type))]
    public EChannelType Type { get; set; }
    
    [MaxLength(64)]
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Name))]
    public string? Name { get; set; }

    public string ChannelName =>
        (Type == EChannelType.Direct ? "(DM) " : "") + 
        (Name ?? string.Join(", ", Members!.Select(m => m.Name)));

    [MaxLength(1024)]
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Description))]
    public string? Description { get; set; }
    
    public Guid? OwnerId { get; set; }
    
    // Nav
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Owner))]
    public AppUser? Owner { get; set; }
    
    public ICollection<ChannelMember>? Members { get; set; }
}