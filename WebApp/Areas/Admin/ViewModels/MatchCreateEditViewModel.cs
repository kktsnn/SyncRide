using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class MatchCreateEditViewModel
{
    public Match Match { get; set; } = default!;
    public SelectList? Route1SelectList { get; set; }
    public SelectList? Route2SelectList { get; set; }
}