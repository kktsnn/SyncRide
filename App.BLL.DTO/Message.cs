using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.BLL.DTO;

public class Message : BaseEntityId
{
    [Required]
    public Guid HeaderId { get; set; }
    
    [Required]
    [MaxLength(1024)]
    [Display(ResourceType = typeof(Resources.Domain.RMessage), Name = nameof(Content))]
    public string Content { get; set; } = default!;
    
    
    // Nav
    [Display(ResourceType = typeof(Resources.Domain.RMessageHeader), Name = nameof(MessageHeader))]
    public MessageHeader? Header { get; set; }
    
    public ICollection<MessageHeader>? Replies { get; set; }
}
