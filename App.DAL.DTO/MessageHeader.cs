using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain;
using Base.Domain;

namespace App.DAL.DTO;

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
    public DAL.DTO.ChannelMember? Sender { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RChannel), Name = nameof(Channel))]
    public DAL.DTO.Channel? Channel { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.RMessageHeader), Name = nameof(Parent))]
    public DAL.DTO.Message? Parent { get; set; }
    
    [InverseProperty("Header")] 
    public DAL.DTO.Message? Message { get; set; }
    
    public ICollection<Recipient>? Recipients { get; set; }
}