using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class LocationCreateEditViewModel
{
    public Location Location { get; set; } = default!;
    public SelectList? RouteSelectList { get; set; }
}