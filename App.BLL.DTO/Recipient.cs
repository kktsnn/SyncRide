using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class Recipient : BaseEntityId
{
    [Required] public Guid MessageHeaderId { get; set; }
    
    [Required] public Guid MemberId { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RRecipient), Name = nameof(IsRead))]
    public bool IsRead { get; set; }
    
    
    //Nav
    [Display(ResourceType = typeof(Resources.Domain.RChannelMember), Name = nameof(ChannelMember))]
    public ChannelMember? Member { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMessageHeader), Name = nameof(MessageHeader))]
    public MessageHeader? MessageHeader { get; set; }
}