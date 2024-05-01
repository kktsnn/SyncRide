using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Message : BaseEntityId
{
    public Guid HeaderId { get; set; }
    
    [MaxLength(1024)]
    public string Content { get; set; } = default!;
    
    
    // Nav
    public MessageHeader? Header { get; set; }
    
    public ICollection<MessageHeader>? Replies { get; set; }
}
