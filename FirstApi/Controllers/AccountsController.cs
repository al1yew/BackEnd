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
using FirstApi.Data;
using Microsoft.AspNetCore.Hosting;
using FirstApi.DTOs.AccountDTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTManager _jWTManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountsController(IJWTManager jWTManager, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context, IMapper mapper)
        {
            _jWTManager = jWTManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => (u.NormalizedEmail == loginDto.EmailUsername.Trim().ToUpperInvariant() || u.NormalizedUserName == loginDto.EmailUsername.Trim().ToUpperInvariant()) && !u.IsAdmin && !u.IsDeleted);

            if (appUser == null)
            {
                return NotFound("Login or password is incorrect");
            }

            IList<string> roles = await _userManager.GetRolesAsync(appUser);

            string token = _jWTManager.GenerateToken(appUser, roles);

            string userName = _jWTManager.GetUserNameByToken(token);

            return Ok(new { token, userName });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            AppUser appUser = _mapper.Map<AppUser>(registerDto);

            await _userManager.CreateAsync(appUser, registerDto.Password);

            await _userManager.AddToRoleAsync(appUser, "Member");

            IList<string> roles = await _userManager.GetRolesAsync(appUser);

            string token = _jWTManager.GenerateToken(appUser, roles);

            string userName = _jWTManager.GetUserNameByToken(token);

            return Ok(new { token, userName });
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
