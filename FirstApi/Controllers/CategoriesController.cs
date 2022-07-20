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

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            //burda uje id lazimdi, cunki bilmeliyik, hansi kategoriyani gormek isteyirik!!!
            //id nin null olmagini yoxlamirig, cunki null gelse, yuxaridaki Gete gedecek, cunki orda id yoxdu, ozu basha dushur!

            return Ok(await _context.Categories.FirstOrDefaultAsync(c => c.Id == id));
        }

        [HttpPut]
        [Route("{id?}")]
        public async Task<IActionResult> Put(int? id, Category category)
        {
            if (id == null) return BadRequest("Id is required");

            if (category.Id != id) return BadRequest("Id is not matching to category's Id!");

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (dbCategory == null) return NotFound("Id is wrong!");

            //useri yoxladig, kechirik dbya

            if (category.IsMain)
            {
                if (category.Image == null) return BadRequest("Image is required!");
            }
            else
            {
                if (category.ParentId == null) return BadRequest("You must choose Main category!");

                if (id == category.ParentId) return BadRequest("Id is the same as id of main category!");

                if (!await _context.Categories.AnyAsync(c => c.Id == category.ParentId && c.Id != id && c.IsMain)) return BadRequest("Main category is incorrect!");
            }

            dbCategory.Name = category.Name.Trim();
            dbCategory.ParentId = category.ParentId;
            dbCategory.Image = category.Image;
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
            dbCategory.IsMain = category.IsMain;

            await _context.SaveChangesAsync();

            return Content("Updated");
        }

        [HttpDelete]
        [Route("{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest("Id is required");

            Category dbCategory = await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (dbCategory == null) return NotFound("Id is wrong!");

            if (dbCategory.IsMain && dbCategory.Children != null)
            {
                List<Category> children = await _context.Categories.Where(c => c.ParentId == id).ToListAsync();
                //ushaglari varsa onlari da silmek lazimdi, bunu daha qisa yazmaq lazimdi amma oxunaqli olsun
                foreach (var item in children)
                {
                    item.IsDeleted = true;
                    item.DeletedAt = DateTime.UtcNow.AddHours(4);
                }
            }

            dbCategory.IsDeleted = true;
            dbCategory.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Content("Deleted");
        }

        [HttpOptions]
        [Route("{id?}")]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest("Id is required");

            Category dbCategory = await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);

            if (dbCategory == null) return NotFound("Id is wrong!");

            if (dbCategory.IsMain && dbCategory.Children != null)
            {
                List<Category> children = await _context.Categories.Where(c => c.ParentId == id).ToListAsync();
                //ushaglari varsa onlari da silmek lazimdi, bunu daha qisa yazmaq lazimdi amma oxunaqli olsun
                foreach (var item in children)
                {
                    item.IsDeleted = false;
                    item.DeletedAt = null;
                }
            }

            dbCategory.IsDeleted = false;
            dbCategory.DeletedAt = null;

            await _context.SaveChangesAsync();

            return Content("Restored");
        }

    }
}
