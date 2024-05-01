using App.BLL.DTO.Identity;
using App.Contracts.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using WebApp.Areas.Admin.ViewModels;
using WebApp.ViewModels;
using Index = System.Index;
using Route = App.Domain.Route;

namespace WebApp.Controllers;

[Authorize]
[Route("/Routes/{routeId}/Locations/{action=Index}/{id?}")]
public class LocationsController : Controller
{
    private readonly IAppBll _bll;
    private readonly UserManager<App.Domain.Identity.AppUser> _userManager;

    private Guid UserId => Guid.Parse(_userManager.GetUserId(User)!);
    
    public LocationsController(IAppBll bll, UserManager<App.Domain.Identity.AppUser> userManager)
    {
        _bll = bll;
        _userManager = userManager;
    }

    // GET: Admin/Locations/Create
    public IActionResult Create(Guid routeId)
    {
        if (!IsRouteOwnedByUser(routeId))
        {
            return NotFound();
        }
        return View(new App.BLL.DTO.Location());
    }

    // POST: Admin/Locations/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid routeId, App.BLL.DTO.Location l)
    {
        if (!IsRouteOwnedByUser(routeId))
        {
            return NotFound();
        }
        
        if (ModelState.IsValid)
        {
            _bll.Locations.AddToRoute(routeId, l);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), nameof(_bll.Routes), new { id = routeId});
        }
        
        return View(l);
    }

    // GET: Admin/Locations/Edit/5
    public async Task<IActionResult> Edit(Guid routeId, Guid? id)
    {
        if (id == null || !IsRouteOwnedByUser(routeId))
        {
            return NotFound();
        }

        var location = await _bll.Locations.FirstOrDefaultAsync(id.Value);
        if (location == null)
        {
            return NotFound();
        }
        
        return View(location);
    }

    // POST: Admin/Locations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid routeId, Guid id, App.BLL.DTO.Location l)
    {
        if (id != l.Id || !IsRouteOwnedByUser(routeId))
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _bll.Locations.UpdateWithRoute(routeId, l);
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Locations.ExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Edit), nameof(_bll.Routes), new { id = routeId});
        }
            
        return View(l);
    }

    // GET: Admin/Locations/Delete/5
    public async Task<IActionResult> Delete(Guid routeId, Guid? id)
    {
        if (id == null || !IsRouteOwnedByUser(routeId))
        {
            return NotFound();
        }

        var location = await _bll.Locations.FirstOrDefaultAsync(id.Value);
        if (location == null)
        {
            return NotFound();
        }

        return View(location);
    }

    // POST: Admin/Locations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid routeId, Guid id)
    {
        if (!IsRouteOwnedByUser(routeId))
        {
            return NotFound();
        }
        
        await _bll.Locations.RemoveAsync(id);

        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Edit), nameof(_bll.Routes), new { id = routeId});
    }

    private bool IsRouteOwnedByUser(Guid routeId)
    {
        return _bll.Routes.IsRouteOwnedByUser(routeId, UserId);
    }
}