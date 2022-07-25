using FirstApi.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Encodings;
using Microsoft.Extensions.Configuration;
using FirstApi.Interfaces;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTManager _jWTManager;

        public AccountsController(IJWTManager jWTManager, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _jWTManager = jWTManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            AppUser appUser = await _userManager.FindByNameAsync("SuperAdmin");

            IList<string> roles = await _userManager.GetRolesAsync(appUser);

            string token = _jWTManager.GenerateToken(appUser, roles);

            string userName = _jWTManager.GetUserNameByToken(token);

            return Ok(new { token });
        }













        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });

        //    AppUser appUser = new AppUser
        //    {
        //        Name = "Super",
        //        SurName = "Admin",
        //        UserName = "SuperAdmin",
        //        Email = "admin@admin"
        //    };

        //    IdentityResult identityResult = await _userManager.CreateAsync(appUser, "Admin123");

        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Content("sdelano");
        //}
    }
}
