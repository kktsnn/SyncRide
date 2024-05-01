using App.BLL.DTO;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Domain.Enums;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    [Route("/Channels/{id?}/{action=Index}")]
    public class ChannelsController : Controller
    {
        private readonly IAppBll _bll;
        private readonly UserManager<AppUser> _userManager;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public ChannelsController(IAppBll bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }

        // GET: Admin/Channels
        public async Task<IActionResult> Index(Guid? id)
        {
            var vm = new ChannelLayoutViewModel
            {
                Channels = (await _bll.Channels.GetAllAsync(UserId)).OrderBy(c => c.Type)
            };

            if (id != null)
            {
                vm.ActiveChannel = vm.Channels.FirstOrDefault(c => c.Id == id.Value);
                vm.ReceivedMessages = await _bll.Recipients.GetAllByChannelAsync(id.Value, UserId);

                foreach (var r in vm.ReceivedMessages)
                {
                    if (r.IsRead) continue;

                    var rec = new Recipient
                    {
                        IsRead = true,
                        Id = r.Id,
                        MemberId = r.MemberId,
                        MessageHeaderId = r.MessageHeaderId
                    };
                    
                    _bll.Recipients.Update(rec);
                }

                await _bll.SaveChangesAsync();
            }

            return View(vm);
        }

        // GET: Admin/Channels/Create
        [Route("/Channels/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Channels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Channels/Create")]
        public async Task<IActionResult> Create(Channel c)
        {
            if (ModelState.IsValid)
            {
                c.Type = EChannelType.Group;
                c.OwnerId = UserId;
                
                var res = _bll.Channels.Add(c);

                var member = new ChannelMember
                {
                    ChannelId = res.Id,
                    AppUserId = UserId
                };

                _bll.ChannelMembers.Add(member);
                
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = res.Id});
            }
            
            return View(c);
        }

        // GET: Admin/Channels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _bll.Channels.FirstOrDefaultAsync(id.Value);
            if (channel == null)
            {
                return NotFound();
            }
            
            return View(channel);
        }

        // POST: Admin/Channels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Channel c)
        {
            if (id != c.Id)
            {
                return NotFound();
            }

            var channel = await _bll.Channels.FirstOrDefaultAsync(id);
            
            if (channel!.OwnerId != UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    channel.Name = c.Name;
                    channel.Description = c.Description;
                    
                    _bll.Channels.Update(channel);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.Channels.ExistsAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(c);
        }

        // GET: Admin/Channels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _bll.Channels.FirstOrDefaultAsync(id.Value, UserId);
            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // POST: Admin/Channels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Channels.RemoveAsync(id, UserId);

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
