using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

public class RemoveIcon : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _env;

    private string UploadDir => _env.WebRootPath;

    public RemoveIcon(UserManager<AppUser> userManager, IWebHostEnvironment env)
    {
        _userManager = userManager;
        _env = env;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = (await _userManager.GetUserAsync(User))!;
            
        var filePath = Path.Combine(UploadDir, "assets", "icons", user.Id + user.ProfilePictureExtension);
        if (Path.Exists(filePath)) System.IO.File.Delete(filePath);

        user.ProfilePictureExtension = null;
            
        await _userManager.UpdateAsync(user);
        
        return RedirectToPage("/Account/Manage/Index");
    }
}