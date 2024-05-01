using Microsoft.AspNetCore.Mvc.Rendering;
using Route = App.DAL.DTO.Route;

namespace WebApp.Areas.Admin.ViewModels;

public class RouteCreateEditViewModel
{
    public Route Route { get; set; } = default!;
    public SelectList? AppUserSelectList { get; set; }
}