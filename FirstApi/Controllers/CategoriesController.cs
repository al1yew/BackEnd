using FirstApi.Data;
using FirstApi.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //hamsini get edirik, id qebul etmir, list sheklinde qaytaririq
            return Ok(await _context.Categories.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category category)
        {
            //bu createdir, hesab etki mvc de get etdik, view gaytardig, sonra inputlari doldurdug,
            //create buttonu basdig, geldi posta. Eyni mentignen postu yazmaliyig, bize kategoriya gelir ve biz onu yoxlayib est edirik

            if (category == null) return BadRequest();

            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.Trim().ToLower())) return Conflict($"{category.Name} Already exists!");

            if (category.IsMain)
            {
                if (category.Image == null) return BadRequest("Image is required!");
            }
            else
            {
                if (category.ParentId == null) return BadRequest("You must choose Main category!");

                if (!await _context.Categories.AnyAsync(c => c.Id == category.ParentId && c.IsMain)) return BadRequest("Main category is incorrect!");
            }

            category.Name = category.Name.Trim();
            category.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return StatusCode(200, category);
            //return Created();
        }



    }
}
