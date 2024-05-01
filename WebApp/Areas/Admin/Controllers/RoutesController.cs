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
using Microsoft.Identity.Client.Region;
using WebApp.Areas.Admin.ViewModels;
using Route = App.Domain.Route;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoutesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public RoutesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Admin/Routes
        public async Task<IActionResult> Index()
        {
            var res = await _uow.Routes.GetAllAsync();
            return View(res);
        }

        // GET: Admin/Routes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _uow.Routes.FirstOrDefaultAsync(id.Value);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: Admin/Routes/Create
        public async Task<IActionResult> Create()
        {
            var vm = new RouteCreateEditViewModel
            {
                AppUserSelectList = new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id), nameof(AppUser.Email)),
            };
            return View(vm);
        }

        // POST: Admin/Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RouteCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _uow.Routes.Add(vm.Route);

                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.AppUserSelectList = new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id), nameof(AppUser.Email), vm.Route.AppUserId);
            return View(vm);
        }

        // GET: Admin/Routes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _uow.Routes.FirstOrDefaultAsync(id.Value);
            if (route == null)
            {
                return NotFound();
            }
            
            var vm = new RouteCreateEditViewModel
            {
                Route = route,
                AppUserSelectList = new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id), nameof(AppUser.Email), route.AppUserId),
            };
            return View(vm);
        }

        // POST: Admin/Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RouteCreateEditViewModel vm)
        {
            if (id != vm.Route.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Routes.Update(vm.Route);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Routes.ExistsAsync(vm.Route.Id))
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
            vm.AppUserSelectList = new SelectList(await _uow.Users.GetAllAsync(), nameof(AppUser.Id), nameof(AppUser.Email), vm.Route.AppUserId);
            
            return View(vm);
        }

        // GET: Admin/Routes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _uow.Routes.FirstOrDefaultAsync(id.Value);
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
            await _uow.Routes.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
