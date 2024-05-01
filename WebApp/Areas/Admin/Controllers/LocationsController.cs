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
using Route = App.Domain.Route;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LocationsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public LocationsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Locations
        public async Task<IActionResult> Index()
        {
            var res = await _uow.Locations.GetAllAsync();
            return View(res);
        }

        // GET: Admin/Locations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _uow.Locations.FirstOrDefaultAsync(id.Value);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Admin/Locations/Create
        public async Task<IActionResult> Create()
        {
            var vm = new LocationCreateEditViewModel
            {
                RouteSelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name))
            };
            return View(vm);
        }

        // POST: Admin/Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _uow.Locations.Add(vm.Location);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.RouteSelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), vm.Location.RouteId);
            return View(vm);
        }

        // GET: Admin/Locations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _uow.Locations.FirstOrDefaultAsync(id.Value);
            if (location == null)
            {
                return NotFound();
            }
            
            var vm = new LocationCreateEditViewModel
            {
                Location = location,
                RouteSelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), location.RouteId)
            };

            return View(vm);
        }

        // POST: Admin/Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LocationCreateEditViewModel vm)
        {
            if (id != vm.Location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Locations.Update(vm.Location);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Locations.ExistsAsync(id))
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

            vm.RouteSelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), vm.Location.RouteId);
            
            return View(vm);
        }

        // GET: Admin/Locations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _uow.Locations.FirstOrDefaultAsync(id.Value);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Admin/Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Locations.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
