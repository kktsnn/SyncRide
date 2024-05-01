using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class ChannelMemberCreateEditViewModel
{
    public ChannelMember ChannelMember { get; set; } = default!;
    public SelectList? AppUserSelectList { get; set; }
    public SelectList? ChannelSelectList { get; set; }
}