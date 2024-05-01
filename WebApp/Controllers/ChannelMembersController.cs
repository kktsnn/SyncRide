using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Domain.Identity;
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
    [Route("/Channels/{channelId}/Members/[action]/{id?}")]
    public class ChannelMembersController : Controller
    {
        private readonly IAppBll _bll;
        private readonly UserManager<AppUser> _userManager;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public ChannelMembersController(IAppBll bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }

        // GET: Admin/ChannelMembers/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ChannelMemberCreateEditViewModel
            {
                MemberSelectList = new SelectList(
                    await _bll.Users.GetAllMatchedUsersAsync(UserId), 
                    nameof(App.DAL.DTO.Identity.AppUser.Id), 
                    nameof(App.BLL.DTO.Identity.AppUser.FullName)
                    )
            };
            return View(vm);
        }

        // POST: Admin/ChannelMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid channelId, ChannelMemberCreateEditViewModel vm)
        {
            if (!await _bll.Channels.ExistsAsync(channelId, UserId)) 
                return RedirectToAction(nameof(Index), nameof(Channel) + "s", new {id = channelId});
            
            if (ModelState.IsValid)
            {
                var member = new ChannelMember
                {
                    AppUserId = vm.AppUserId,
                    ChannelId = channelId
                };
                
                _bll.ChannelMembers.Add(member);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index), nameof(Channel) + "s", new {id = channelId});
            }

            vm.MemberSelectList = new SelectList(
                await _bll.Users.GetAllMatchedUsersAsync(UserId),
                nameof(App.DAL.DTO.Identity.AppUser.Id),
                nameof(App.BLL.DTO.Identity.AppUser.FullName)
            );

            return View(vm);
        }

        // GET: Admin/ChannelMembers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelMember = await _bll.ChannelMembers.FirstOrDefaultAsync(id.Value);
            if (channelMember == null || channelMember.Channel!.OwnerId != UserId)
            {
                return NotFound();
            }

            var vm = new ChannelMemberCreateEditViewModel
            {
                IsOwner = channelMember.AppUserId == channelMember.Channel.OwnerId,
                Id = channelMember.Id,
                AppUserId = channelMember.AppUserId,
                Nickname = channelMember.Nickname
            };
            
            return View(vm);
        }

        // POST: Admin/ChannelMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid channelId, Guid id, ChannelMemberCreateEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            var member = await _bll.ChannelMembers.FirstOrDefaultAsync(id);
            
            if (member!.Channel!.OwnerId != UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    member.Nickname = vm.Nickname;
                    
                    _bll.ChannelMembers.Update(member);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.ChannelMembers.ExistsAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), nameof(Channel) + "s", new {id = channelId});
            }
            
            return View(vm);
        }
        
        public async Task<IActionResult> Kick(Guid channelId, Guid id, ChannelMemberCreateEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }
            
            var member = await _bll.ChannelMembers.FirstOrDefaultAsync(id);
            if (member!.Channel!.OwnerId == UserId)
            {
                await _bll.ChannelMembers.RemoveAsync(member);
                await _bll.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index), nameof(Channel) + "s", new {id = channelId});
        }

        public async Task<IActionResult> MakeOwner(Guid channelId, Guid id, ChannelMemberCreateEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }
            
            var channel = await _bll.Channels.FirstOrDefaultAsync(channelId);
            
            if (channel != null && channel.OwnerId == UserId)
            {
                channel.OwnerId = vm.AppUserId;
                channel.Owner = null; // SHOULD CREATE NEW OBJECT!
                
                _bll.Channels.Update(channel);
                await _bll.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index), nameof(Channel) + "s", new {id = channelId});
        }
    }
}
