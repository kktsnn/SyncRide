using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.BLL.DTO;

public class MessageHeader : BaseEntityId
{
    [Required]
    public Guid SenderId { get; set; }
    
    [Required] 
    public Guid ChannelId { get; set; }
    
    [Required]
    [Display(ResourceType = typeof(Resources.Domain.RMessageHeader), Name = nameof(SentAt))]
    public DateTime SentAt { get; set; }
    
    public Guid? ParentId { get; set; }
    
    
    // Nav
    [Display(ResourceType = typeof(Resources.Domain.RMessageHeader), Name = nameof(Sender))]
    public ChannelMember? Sender { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Channel))]
    public Channel? Channel { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMessageHeader), Name = nameof(Parent))]
    public Message? Parent { get; set; }
    
    [InverseProperty("Header")] 
    public Message? Message { get; set; }
    
    public ICollection<Recipient>? Recipients { get; set; }
}