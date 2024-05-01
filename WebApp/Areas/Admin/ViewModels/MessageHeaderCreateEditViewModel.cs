using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class MessageHeaderCreateEditViewModel
{
    public MessageHeader MessageHeader { get; set; } = default!;
    public SelectList? ChannelSelectList { get; set; }
    public SelectList? ParentMessageSelectList { get; set; }
    public SelectList? SenderSelectList { get; set; }
}