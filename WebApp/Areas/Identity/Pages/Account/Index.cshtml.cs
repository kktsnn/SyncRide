using System.ComponentModel.DataAnnotations;
using App.Contracts.BLL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
#nullable disable

namespace WebApp.Areas.Identity.Pages.Account;

public class IndexModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppBll _bll;
    
    public IndexModel(UserManager<AppUser> userManager, IAppBll bll)
    {
        _userManager = userManager;
        _bll = bll;
    }
    
    public UserModel UserInfo { get; set; }
    
    public class UserModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Display(Name = "About")]
        public string About { get; set; }
        
        public string IconPath { get; set; }
    }

    private void LoadData(AppUser user)
    {
        UserInfo = new UserModel
        {
            Username = user.UserName,
            PhoneNumber = user.PhoneNumber,
            FirstName = user.FirstName,
            LastName = user.LastName,
            About = user.About,
            IconPath = "/Assets/Icons/" + (
                user.ProfilePictureExtension != null ? 
                user.Id + user.ProfilePictureExtension : 
                "default.png"
            )
        };
    }
    
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        AppUser user;
        
        if (id == null)
        {
            user = await _userManager.GetUserAsync(User);
            
            LoadData(user);
            
            return Page();
        }
        
        if (!await UsersKnowEachOther(id.Value))
        {
            return NotFound("User not found");
        }
        
        user = await _userManager.FindByIdAsync(id.Value.ToString());
        
        LoadData(user);
        return Page();
    }

    private async Task<bool> UsersKnowEachOther(Guid id)
    {
        var userId = Guid.Parse(_userManager.GetUserId(User)!);
        if (id == userId) return true;
        
        return await _bll.Users.AreUsersInChannel(id, userId) &&
            await _bll.Matches.FindMatchBetweenUsersAsync(userId, id) != null;
    }
}