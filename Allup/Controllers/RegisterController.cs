using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.RegisterViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Controllers
{
    //[Authorize(Roles = "Member")]
    public class RegisterController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RegisterController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.SurName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            result = await _userManager.AddToRoleAsync(appUser, "Member");

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Login()
        {
            //return View(await _context.BrandSliders.ToListAsync());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or Password is wrong!");
                return View(loginVM);
            }

            if (appUser.IsAdmin)
            {
                ModelState.AddModelError("", "You cannot sign in!");
                return View(loginVM);
            }

            if (!await _userManager.CheckPasswordAsync(appUser, loginVM.Password))
            {
                ModelState.AddModelError("", "Email or Password is wrong!");
                return View(loginVM);
            }

            await _signInManager.SignInAsync(appUser, loginVM.RememberMe);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }



        #region Created Roles

        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

        //    return Content("Salammmmm");
        //}

        #endregion

        #region Created Super Admin

        //public async Task<IActionResult> CreateSuperAdmin()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        Name = "Super",
        //        Surname = "Admin",
        //        UserName = "SuperAdmin",
        //        Email = "vasifja@code.edu.az"
        //    };

        //    appUser.IsAdmin = true;

        //    await _userManager.CreateAsync(appUser, "SuperAdmin@12345");

        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Content("Super admin est ");
        //}
        #endregion

    }
}
