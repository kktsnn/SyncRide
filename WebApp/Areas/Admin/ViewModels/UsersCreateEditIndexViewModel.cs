using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;

namespace WebApp.Areas.Admin.ViewModels;

public class UsersCreateEditIndexViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(Email))]
    public string Email { get; set; } = default!;
    
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(FirstName))]
    public string FirstName { get; set; } = default!;
    
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;
    
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(Password))]
    public string? Password { get; set; }
    
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(Role))]
    public EAppRole? Role { get; set; }
    
    [Display(ResourceType = typeof(Base.Resources.Domain.Identity.RAppUser), Name = nameof(Roles))]
    public string? Roles { get; set; }

    public string? IconButton { get; set; }

}
