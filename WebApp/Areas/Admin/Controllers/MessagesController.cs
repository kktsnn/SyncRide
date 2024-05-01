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
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MessagesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public MessagesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Messages
        public async Task<IActionResult> Index()
        {
            var res = await _uow.Messages.GetAllAsync();
            return View(res);
        }

        // GET: Admin/Messages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _uow.Messages.FirstOrDefaultAsync(id.Value);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Admin/Messages/Create
        public async Task<IActionResult> Create()
        {
            var vm = new MessageCreateEditViewModel
            {
                HeaderSelectList = new SelectList(await _uow.MessageHeaders.GetAllAsync(), nameof(MessageHeader.Id), nameof(MessageHeader.Id))
            };
            
            return View(vm);
        }

        // POST: Admin/Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _uow.Messages.Add(vm.Message);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.HeaderSelectList = 
                new SelectList(await _uow.MessageHeaders.GetAllAsync(), nameof(MessageHeader.Id), nameof(MessageHeader.Id), vm.Message.HeaderId);
            return View(vm);
        }

        // GET: Admin/Messages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _uow.Messages.FirstOrDefaultAsync(id.Value);
            if (message == null)
            {
                return NotFound();
            }

            var vm = new MessageCreateEditViewModel
            {
                Message = message,
                HeaderSelectList = 
                    new SelectList(await _uow.MessageHeaders.GetAllAsync(), nameof(MessageHeader.Id), nameof(MessageHeader.Id), message.HeaderId)
            };

            return View(vm);
        }

        // POST: Admin/Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MessageCreateEditViewModel vm)
        {
            if (id != vm.Message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Messages.Update(vm.Message);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Messages.ExistsAsync(id))
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
            
            vm.HeaderSelectList = 
                new SelectList(await _uow.MessageHeaders.GetAllAsync(), nameof(MessageHeader.Id), nameof(MessageHeader.Id), vm.Message.HeaderId);
            
            return View(vm);
        }

        // GET: Admin/Messages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _uow.Messages.FirstOrDefaultAsync(id.Value);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Admin/Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Messages.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
