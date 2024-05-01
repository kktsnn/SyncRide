using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;
using Route = App.Domain.Route;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ChannelMembersController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public ChannelMembersController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/ChannelMembers
        public async Task<IActionResult> Index()
        {
            var res = await _uow.ChannelMembers.GetAllAsync();
            return View(res);
        }

        // GET: Admin/ChannelMembers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelMember = await _uow.ChannelMembers.FirstOrDefaultAsync(id.Value);
            if (channelMember == null)
            {
                return NotFound();
            }

            return View(channelMember);
        }

        // GET: Admin/ChannelMembers/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ChannelMemberCreateEditViewModel
            {
                AppUserSelectList = new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id), nameof(AppUser.Email)),
                ChannelSelectList = new SelectList(await _uow.Channels.GetAllAsync(), nameof(Channel.Id), nameof(Channel.Name)),
            };
            
            return View(vm);
        }

        // POST: Admin/ChannelMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChannelMemberCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _uow.ChannelMembers.Add(vm.ChannelMember);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.AppUserSelectList = new SelectList(
                await _uow.Users.GetAllAsync(), 
                nameof(AppUser.Id), 
                nameof(AppUser.Email), 
                vm.ChannelMember.AppUserId
                );
            
            vm.ChannelSelectList = new SelectList(
                await _uow.Channels.GetAllAsync(), 
                nameof(Channel.Id), 
                nameof(Channel.Name), 
                vm.ChannelMember.ChannelId
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

            var channelMember = await _uow.ChannelMembers.FirstOrDefaultAsync(id.Value);
            if (channelMember == null)
            {
                return NotFound();
            }

            var vm = new ChannelMemberCreateEditViewModel()
            {
                ChannelMember = channelMember,
                AppUserSelectList = new SelectList(
                    await _uow.Users.GetAllAsync(),
                    nameof(AppUser.Id),
                    nameof(AppUser.Email),
                    channelMember.AppUserId
                ),

                ChannelSelectList = new SelectList(
                    await _uow.Channels.GetAllAsync(),
                    nameof(Channel.Id),
                    nameof(Channel.Name),
                    channelMember.ChannelId
                )
            };
            
            return View(vm);
        }

        // POST: Admin/ChannelMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ChannelMemberCreateEditViewModel vm)
        {
            if (id != vm.ChannelMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.ChannelMembers.Update(vm.ChannelMember);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.ChannelMembers.ExistsAsync(id))
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
            vm.AppUserSelectList = new SelectList(
                await _uow.Users.GetAllAsync(), 
                nameof(AppUser.Id), 
                nameof(AppUser.Email), 
                vm.ChannelMember.AppUserId
            );
            
            vm.ChannelSelectList = new SelectList(
                await _uow.Channels.GetAllAsync(), 
                nameof(Channel.Id), 
                nameof(Channel.Name), 
                vm.ChannelMember.ChannelId
            );
            return View(vm);
        }

        // GET: Admin/ChannelMembers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelMember = await _uow.ChannelMembers.FirstOrDefaultAsync(id.Value);
            if (channelMember == null)
            {
                return NotFound();
            }

            return View(channelMember);
        }

        // POST: Admin/ChannelMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.ChannelMembers.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
