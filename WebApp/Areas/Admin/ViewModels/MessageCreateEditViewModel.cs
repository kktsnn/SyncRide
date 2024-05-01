using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class MessageCreateEditViewModel
{
    public Message Message { get; set; } = default!;
    public SelectList? HeaderSelectList { get; set; }
}