using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class MessageHeader : BaseEntityId
{
    public Guid SenderId { get; set; }
    
    public Guid ChannelId { get; set; }
    
    public DateTime SentAt { get; set; }
    
    public DateTime? EditedAt { get; set; }
    
    public Guid? ParentId { get; set; }
    
    
    // Nav
    public ChannelMember? Sender { get; set; }
    
    public Channel? Channel { get; set; }
    
    public Message? Parent { get; set; }
    
    [InverseProperty("Header")] 
    public Message? Message { get; set; }
    
    public ICollection<Recipient>? Recipients { get; set; }
}