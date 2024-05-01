using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class ChannelCreateEditViewModel
{
    public Channel Channel { get; set; } = default!;
    public SelectList? AppUserSelectList { get; set; }
}