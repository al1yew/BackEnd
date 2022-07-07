using Allup.Areas.Manage.ViewModels.SettingViewModels;
using Allup.DAL;
using Allup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IDictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] SettingVM settingVM)
        {
            List<Setting> settings = await _context.Settings.ToListAsync();

            if (!settings.Any(x => x.Key == settingVM.key))
            {
                return BadRequest();
            }

            settings.FirstOrDefault(x => x.Key == settingVM.key).Value = settingVM.value;

            await _context.SaveChangesAsync();

            return PartialView("_SettingIndexPartial", settings.ToDictionary(x => x.Key, x => x.Value));
        }
    }
}
