using Allup.DAL;
using Allup.Models;
using Allup.ViewModels.RegisterViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
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

            return Content("ok");
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

        //    await _userManager.CreateAsync(appUser, "SuperAdmin@12345");

        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Content("Super admin est ");
        //}
        #endregion

    }
}
