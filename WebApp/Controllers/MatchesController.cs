using App.BLL.DTO;
using App.BLL.DTO.Identity;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;
using Channel = App.Domain.Channel;
using Route = App.Domain.Route;

namespace WebApp.Controllers
{
    [Authorize]
    public class MatchesController : Controller
    {
        private readonly IAppBll _bll;
        private readonly UserManager<App.Domain.Identity.AppUser> _userManager;
        
        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public MatchesController(IAppBll bll, UserManager<App.Domain.Identity.AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }


        // GET: Admin/Matches
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Matches.GetAllVisibleAsync(UserId);
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(Guid? id)
        {
            if (id == null)
            {
                return View(nameof(Index));
            }

            var match = await _bll.Matches.Accept(id.Value, Guid.Parse(_userManager.GetUserId(User)!));

            if (match.Accepted)
            {
                var channel = await _bll.Channels.DmExistsBetweenUsers(
                    match.Route1!.AppUserId, match.Route2!.AppUserId);

                if (channel == null)
                {
                    channel = _bll.Channels.Add(new App.BLL.DTO.Channel()
                    {
                        Type = EChannelType.Direct
                    });

                    _bll.ChannelMembers.Add(new ChannelMember()
                    {
                        ChannelId = channel.Id,
                        AppUserId = match.Route1!.AppUserId
                    });
                    
                    _bll.ChannelMembers.Add(new ChannelMember()
                    {
                        ChannelId = channel.Id,
                        AppUserId = match.Route2!.AppUserId
                    });
                    
                }
                
                await _bll.SaveChangesAsync();
                
                return RedirectToAction("Details", nameof(Channel) + "s", new {id = channel.Id});
            }

            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
