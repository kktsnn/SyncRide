using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Recipient : BaseEntityId
{
    public Guid MessageHeaderId { get; set; }
    
    public Guid MemberId { get; set; }
    
    public bool IsRead { get; set; }
    
    
    //Nav
    public ChannelMember? Member { get; set; }
    
    public MessageHeader? MessageHeader { get; set; }
}