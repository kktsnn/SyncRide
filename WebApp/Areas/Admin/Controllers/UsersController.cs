using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Enums;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(EAppRole.Admin))]
    public class UsersController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        private string UploadDir => _env.WebRootPath;

        public UsersController(IAppUnitOfWork uow, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _uow = uow;
            _userManager = userManager;
            _env = env;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var vms = new List<UsersCreateEditIndexViewModel>();
            
            foreach (var user in await _uow.Users.GetAllAsync())
            {
                var vm = await _userManager
                    .GetRolesAsync((await _userManager.FindByIdAsync(user.Id.ToString()))!)
                    .ContinueWith(t => new UsersCreateEditIndexViewModel
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email!,
                            Roles = string.Join(", ", t.Result)
                        }
                    );

                vms.Add(vm);
            }
            
            return View(vms);
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            var appUser = await _uow.Users
                .FirstOrDefaultAsync(id.Value);
            if (appUser == null)
            {
                return NotFound();
            }
        
            return View(appUser);
        }
        
        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View(new UsersCreateEditIndexViewModel());
        }
        
        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsersCreateEditIndexViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Email = vm.Email,
                    UserName = vm.Email,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName
                };

                if (string.IsNullOrEmpty(vm.Password))
                {
                    ModelState.AddModelError(nameof(vm.Password), "Password is required!");
                    return View(vm);
                }

                var res = await _userManager.CreateAsync(user, vm.Password!);
                
                if (res.Succeeded) return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError(nameof(vm.Password), res.ToString());
                return View(vm);

            }
            return View(vm);
        }
        
        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            var appUser = await _uow.Users.FirstOrDefaultAsync(id.Value);
            if (appUser == null)
            {
                return NotFound();
            }
            
            var roles = await _userManager.GetRolesAsync((await _userManager.GetUserAsync(User))!);

            var vm = new UsersCreateEditIndexViewModel()
            {
                Id = id.Value,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email!,
                Roles = string.Join(", ", roles)
            };
            
            return View(vm);
        }
        
        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UsersCreateEditIndexViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }
        
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id.ToString());

                    if (user == null)
                    {
                        return NotFound();
                    }
                    
                    if (!string.IsNullOrEmpty(vm.Password))
                    {
                        
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, token, vm.Password);
                    }

                    user.FirstName = vm.FirstName;
                    user.LastName = vm.LastName;
                    user.Email = vm.Email;
                    user.UserName = vm.Email;
                    
                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Users.ExistsAsync(id))
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
            return View(vm);
        }
        
        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            var appUser = await _uow.Users
                .FirstOrDefaultAsync(id.Value);
            if (appUser == null)
            {
                return NotFound();
            }
        
            return View(appUser);
        }
        
        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appUser = await _userManager.FindByIdAsync(id.ToString());
            if (appUser != null)
            {
                await _userManager.DeleteAsync(appUser);
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Icon(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            
            var filePath = Path.Combine(UploadDir, "assets", "icons", id + user!.ProfilePictureExtension);
            if (Path.Exists(filePath)) System.IO.File.Delete(filePath);

            user.ProfilePictureExtension = null;
            
            await _userManager.UpdateAsync(user);
        
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Role(Guid id, UsersCreateEditIndexViewModel vm)
        {
            if (vm.Role == null)
            {
                return RedirectToAction(nameof(Index));
            }
            
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (vm.IconButton == "add")
            {
                await _userManager.AddToRoleAsync(user!, vm.Role.ToString()!);
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user!, vm.Role.ToString()!);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
