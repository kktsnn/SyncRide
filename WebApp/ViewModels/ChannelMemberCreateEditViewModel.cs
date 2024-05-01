using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class ChannelMemberCreateEditViewModel
{
    public bool? IsOwner { get; set; }
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public string? Nickname { get; set; }
    public SelectList? MemberSelectList { get; set; }
}