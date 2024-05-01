using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Domain;
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
    public class RoutesController : Controller
    {
        private readonly IAppBll _bll;
        private readonly UserManager<AppUser> _userManager;

        private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);

        public RoutesController(IAppBll bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }

        // GET: Admin/Routes
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Routes.GetAllAsync(UserId);
            return View(res);
        }

        // GET: Admin/Routes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || !IsRouteOwnedByUser(id.Value))
            {
                return NotFound();
            }

            var route = await _bll.Routes.FirstOrDefaultAsync(id.Value);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: Admin/Routes/Create
        public IActionResult Create()
        {
            return View(new App.BLL.DTO.Route());
        }

        // POST: Admin/Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(App.BLL.DTO.Route r)
        {
            if (ModelState.IsValid)
            {
                _bll.Routes.AddToUser(UserId, r);

                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(r);
        }

        // GET: Admin/Routes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !IsRouteOwnedByUser(id.Value))
            {
                return NotFound();
            }

            var route = await _bll.Routes.FirstOrDefaultAsync(id.Value);
            if (route == null)
            {
                return NotFound();
            }
            
            return View(route);
        }

        // POST: Admin/Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, App.BLL.DTO.Route r)
        {
            if (id != r.Id || !IsRouteOwnedByUser(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.Routes.UpdateForUser(UserId, r);

                    r.Locations = (await _bll.Locations.GetAllByRouteIdAsync(r.Id)).ToList();
                    
                    await RecalculateMatches(UserId, r);
                    
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bll.Routes.ExistsAsync(r.Id))
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

            return View(r);
        }

        private async Task RecalculateMatches(Guid userId, App.BLL.DTO.Route route)
        {
            await _bll.Matches.DeleteAllByRouteAsync(route.Id);

            foreach (var r in await _bll.Routes.GetAllNotOwnedByUserAsync(userId))
            {
                var match = _bll.Matches.CalculateMatch(route, r);
                if (match.MatchPercent > .8) _bll.Matches.Add(match);
            }
        }

        // GET: Admin/Routes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || !IsRouteOwnedByUser(id.Value))
            {
                return NotFound();
            }

            var route = await _bll.Routes.FirstOrDefaultAsync(id.Value);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Admin/Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!IsRouteOwnedByUser(id))
            {
                return NotFound();
            }
            
            await _bll.Routes.RemoveAsync(id);

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool IsRouteOwnedByUser(Guid routeId)
        {
            return _bll.Routes.IsRouteOwnedByUser(routeId, UserId);
        }
    }
}
