using App.Contracts.BLL;
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;
using Channel = App.BLL.DTO.Channel;

namespace WebApp.Controllers
{
    [Authorize]
    [Route("/Channels/{channelId}/Messages/[action]/{id?}")]
    public class MessagesController : Controller
    {
        private readonly IAppBll _bll;
        private readonly UserManager<AppUser> _userManager;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public MessagesController(IAppBll bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid channelId, string content, Guid? parentId = null)
        {
            // TODO: Check if user in channel
            
            var channel = await _bll.Channels.FirstOrDefaultAsync(channelId, UserId);
            
            var header = new MessageHeader
            {
                ChannelId = channelId,
                ParentId = parentId,
                SenderId = channel!.Members!.FirstOrDefault(m => m.AppUserId == UserId)!.Id,
                SentAt = DateTime.UtcNow
            };

            header = _bll.MessageHeaders.Add(header);

            var message = new Message
            {
                HeaderId = header.Id,
                Content = content
            };

            _bll.Messages.Add(message);

            foreach (var cm in channel.Members!)
            {
                var recipient = new Recipient
                {
                    MemberId = cm.Id,
                    MessageHeaderId = header.Id,
                    IsRead = cm.AppUserId == UserId
                };
                _bll.Recipients.Add(recipient);
            }

            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index), nameof(Channel) + "s", new { id = channelId });
        }
        
        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid channelId, Guid id)
        {
            var channel = await _bll.Channels.FirstOrDefaultAsync(channelId);

            if (channel != null && channel.OwnerId == UserId)
            {
                var recipient = await _bll.Recipients.FirstOrDefaultAsync(id, UserId);
                
                await _bll.MessageHeaders.RemoveAsync(recipient!.MessageHeader!.Id);
            
                await _bll.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index), nameof(Channel) + "s", new { id = channelId });
        }
    }
}
