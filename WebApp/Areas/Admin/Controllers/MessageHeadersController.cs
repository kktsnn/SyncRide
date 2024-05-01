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
using App.DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MessageHeadersController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public MessageHeadersController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/MessageHeaders
        public async Task<IActionResult> Index()
        {
            var res = await _uow.MessageHeaders.GetAllAsync();
            return View(res);
        }

        // GET: Admin/MessageHeaders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageHeader = await _uow.MessageHeaders.FirstOrDefaultAsync(id.Value);
            if (messageHeader == null)
            {
                return NotFound();
            }

            return View(messageHeader);
        }

        // GET: Admin/MessageHeaders/Create
        public async Task<IActionResult> Create()
        {
            var vm = new MessageHeaderCreateEditViewModel
            {
                ChannelSelectList = new SelectList(await _uow.Channels.GetAllAsync(), nameof(Channel.Id), nameof(Channel.Name)),
                ParentMessageSelectList = new SelectList(await _uow.Messages.GetAllAsync(), nameof(Message.Id), nameof(Message.Id)),
                SenderSelectList = new SelectList(await _uow.ChannelMembers.GetAllAsync(), nameof(ChannelMember.Id), nameof(ChannelMember.Name))
            };
            
            return View(vm);
        }

        // POST: Admin/MessageHeaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageHeaderCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _uow.MessageHeaders.Add(vm.MessageHeader);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.ChannelSelectList =
                new SelectList(await _uow.Channels.GetAllAsync(), nameof(Channel.Id), nameof(Channel.Name), vm.MessageHeader.ChannelId);
            vm.ParentMessageSelectList =
                new SelectList(await _uow.Messages.GetAllAsync(), nameof(Message.Id), nameof(Message.Id), vm.MessageHeader.ParentId);
            vm.SenderSelectList = 
                new SelectList(await _uow.ChannelMembers.GetAllAsync(), nameof(ChannelMember.Id), nameof(ChannelMember.Name), vm.MessageHeader.SenderId);
            
            return View(vm);
        }

        // GET: Admin/MessageHeaders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageHeader = await _uow.MessageHeaders.FirstOrDefaultAsync(id.Value);
            if (messageHeader == null)
            {
                return NotFound();
            }
            
            var vm = new MessageHeaderCreateEditViewModel
            {
                MessageHeader = messageHeader,
                ChannelSelectList = new SelectList(await _uow.Channels.GetAllAsync(), nameof(Channel.Id), nameof(Channel.Name), messageHeader.ChannelId),
                ParentMessageSelectList = new SelectList(await _uow.Messages.GetAllAsync(), nameof(Message.Id), nameof(Message.Id), messageHeader.ParentId),
                SenderSelectList = new SelectList(await _uow.ChannelMembers.GetAllAsync(), nameof(ChannelMember.Id), nameof(ChannelMember.Name), messageHeader.SenderId)
            };
            
            return View(vm);
        }

        // POST: Admin/MessageHeaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MessageHeaderCreateEditViewModel vm)
        {
            if (id != vm.MessageHeader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.MessageHeaders.Update(vm.MessageHeader);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.MessageHeaders.ExistsAsync(id))
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
            vm.ChannelSelectList =
                new SelectList(await _uow.Channels.GetAllAsync(), nameof(Channel.Id), nameof(Channel.Name), vm.MessageHeader.ChannelId);
            vm.ParentMessageSelectList =
                new SelectList(await _uow.Messages.GetAllAsync(), nameof(Message.Id), nameof(Message.Id), vm.MessageHeader.ParentId);
            vm.SenderSelectList = 
                new SelectList(await _uow.ChannelMembers.GetAllAsync(), nameof(ChannelMember.Id), nameof(ChannelMember.Name), vm.MessageHeader.SenderId);

            return View(vm);
        }

        // GET: Admin/MessageHeaders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageHeader = await _uow.MessageHeaders.FirstOrDefaultAsync(id.Value);
            if (messageHeader == null)
            {
                return NotFound();
            }

            return View(messageHeader);
        }

        // POST: Admin/MessageHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.MessageHeaders.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
