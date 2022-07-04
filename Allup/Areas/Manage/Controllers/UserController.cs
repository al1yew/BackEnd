using Allup.DAL;
using Allup.Models;
using Allup.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allup.Areas.Manage.ViewModels.AccountViewModels;
using Microsoft.EntityFrameworkCore;

namespace Allup.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<AppUser> query = _userManager.Users.Where(u => u.UserName != User.Identity.Name);

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }

            int itemCount = int.Parse(_context.Settings.FirstOrDefault(s => s.Key == "PageItemsCount").Value);

            ViewBag.Status = status;

            return View(PaginationList<AppUser>.Create(query, page, itemCount));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null) return NotFound();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id, ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid) return View();

            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null) return NotFound();

            if (await _userManager.CheckPasswordAsync(appUser, resetPasswordVM.Password)) return BadRequest();

            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            //string emailConfirm = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }

            return RedirectToAction("Index");
        }
    }
}
