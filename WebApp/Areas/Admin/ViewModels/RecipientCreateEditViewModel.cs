using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class RecipientCreateEditViewModel
{
    public Recipient Recipient { get; set; } = default!;
    public SelectList? MemberSelectList { get; set; }
    public SelectList? MessageHeaderSelectList { get; set; }
}