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
    public class MatchesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public MatchesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }


        // GET: Admin/Matches
        public async Task<IActionResult> Index()
        {
            var res = await _uow.Matches.GetAllAsync();
            return View(res);
        }

        // GET: Admin/Matches/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _uow.Matches.FirstOrDefaultAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Admin/Matches/Create
        public async Task<IActionResult> Create()
        {
            var vm = new MatchCreateEditViewModel
            {
                Route1SelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name)),
                Route2SelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name))
            };

            return View(vm);
        }

        // POST: Admin/Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Match.Route1Id.Equals(vm.Match.Route2Id))
                {
                    _uow.Matches.Add(vm.Match);
                    await _uow.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(nameof(Match), "Routes can not be the same");
            }

            vm.Route1SelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), vm.Match.Route1Id);
            vm.Route2SelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), vm.Match.Route2Id);
            
            return View(vm);
        }

        // GET: Admin/Matches/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _uow.Matches.FirstOrDefaultAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            var vm = new MatchCreateEditViewModel
            {
                Match = match,
                Route1SelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), match.Route1Id),
                Route2SelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), match.Route2Id),
            };
            
            return View(vm);
        }

        // POST: Admin/Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MatchCreateEditViewModel vm)
        {
            if (id != vm.Match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Matches.Update(vm.Match);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Matches.ExistsAsync(id))
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
            vm.Route1SelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), vm.Match.Route1Id);
            vm.Route2SelectList = new SelectList(await _uow.Routes.GetAllAsync(), nameof(Route.Id), nameof(Route.Name), vm.Match.Route2Id);
            return View(vm);
        }

        // GET: Admin/Matches/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _uow.Matches.FirstOrDefaultAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Admin/Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Matches.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
