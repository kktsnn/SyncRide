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

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ChannelsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public ChannelsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Channels
        public async Task<IActionResult> Index()
        {
            var res = await _uow.Channels.GetAllAsync();
            return View(res);
        }

        // GET: Admin/Channels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _uow.Channels.FirstOrDefaultAsync(id.Value);
            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // GET: Admin/Channels/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ChannelCreateEditViewModel
            {
                AppUserSelectList = new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id), nameof(AppUser.Email))
            };
            return View(vm);
        }

        // POST: Admin/Channels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChannelCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _uow.Channels.Add(vm.Channel);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.AppUserSelectList = new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id), nameof(AppUser.Email), vm.Channel.OwnerId);
            
            return View(vm);
        }

        // GET: Admin/Channels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _uow.Channels.FirstOrDefaultAsync(id.Value);
            if (channel == null)
            {
                return NotFound();
            }

            var vm = new ChannelCreateEditViewModel
            {
                Channel = channel,
                AppUserSelectList = 
                    new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id), nameof(AppUser.Email), channel.OwnerId)
            };
            
            return View(vm);
        }

        // POST: Admin/Channels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ChannelCreateEditViewModel vm)
        {
            if (id != vm.Channel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Channels.Update(vm.Channel);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Channels.ExistsAsync(id))
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

            vm.AppUserSelectList = new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id),
                nameof(AppUser.Email), vm.Channel.OwnerId);
            
            return View(vm);
        }

        // GET: Admin/Channels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _uow.Channels.FirstOrDefaultAsync(id.Value);
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
            await _uow.Channels.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
