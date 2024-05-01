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
using Message = NuGet.Protocol.Plugins.Message;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RecipientsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public RecipientsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Recipients
        public async Task<IActionResult> Index()
        {
            var res = await _uow.Recipients.GetAllAsync();
            return View(res);
        }

        // GET: Admin/Recipients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _uow.Recipients.FirstOrDefaultAsync(id.Value);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        // GET: Admin/Recipients/Create
        public async Task<IActionResult> Create()
        {
            var vm = new RecipientCreateEditViewModel
            {
                MemberSelectList = new SelectList(await _uow.ChannelMembers.GetAllAsync(), 
                    nameof(ChannelMember.Id), nameof(ChannelMember.Name)),
                MessageHeaderSelectList = new SelectList(await _uow.MessageHeaders.GetAllAsync(), 
                    nameof(MessageHeader.Id), nameof(MessageHeader.Id))
            };
            
            return View(vm);
        }

        // POST: Admin/Recipients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipientCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _uow.Recipients.Add(vm.Recipient);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.MemberSelectList = 
                new SelectList(await _uow.ChannelMembers.GetAllAsync(), nameof(ChannelMember.Id), 
                    nameof(ChannelMember.Name), vm.Recipient.MemberId);
            vm.MessageHeaderSelectList = 
                new SelectList(await _uow.MessageHeaders.GetAllAsync(), nameof(MessageHeader.Id), 
                    nameof(MessageHeader.Id), vm.Recipient.MessageHeaderId);
            
            return View(vm);
        }

        // GET: Admin/Recipients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _uow.Recipients.FirstOrDefaultAsync(id.Value);
            if (recipient == null)
            {
                return NotFound();
            }
            
            var vm = new RecipientCreateEditViewModel
            {
                Recipient = recipient,
                MemberSelectList = 
                    new SelectList(await _uow.ChannelMembers.GetAllAsync(), nameof(ChannelMember.Id), 
                        nameof(ChannelMember.Name), recipient.MemberId),
                MessageHeaderSelectList = 
                    new SelectList(await _uow.MessageHeaders.GetAllAsync(), nameof(MessageHeader.Id), 
                        nameof(MessageHeader.Id), recipient.MessageHeaderId)
            };
            
            return View(vm);
        }

        // POST: Admin/Recipients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RecipientCreateEditViewModel vm)
        {
            if (id != vm.Recipient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Recipients.Update(vm.Recipient);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _uow.Recipients.ExistsAsync(id))
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

            vm.MemberSelectList =
                new SelectList(await _uow.ChannelMembers.GetAllAsync(), nameof(ChannelMember.Id),
                    nameof(ChannelMember.Name), vm.Recipient.MemberId);
            vm.MessageHeaderSelectList =
                new SelectList(await _uow.MessageHeaders.GetAllAsync(), nameof(MessageHeader.Id),
                    nameof(MessageHeader.Id), vm.Recipient.MessageHeaderId);
            
            return View(vm);
        }

        // GET: Admin/Recipients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _uow.Recipients.FirstOrDefaultAsync(id.Value);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        // POST: Admin/Recipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Recipients.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
