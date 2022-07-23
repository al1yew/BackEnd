using FirstApi.Data;
using FirstApi.Data.Entities;
using FirstApi.DTOs.CategoryDTOs;
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


            #region dede boba
            //----------------------------------dede boba metod
            //List<Category> categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            //List<CategoryListDto> categoryListDtos = new List<CategoryListDto>();

            //foreach (Category item in categories)
            //{
            //    CategoryListDto categoryListDto = new CategoryListDto
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        ParentCategory = item.IsMain ? null : item.Parent.Name
            //    };

            //    categoryListDtos.Add(categoryListDto);
            //}
            #endregion

            #region Dede boba amma biraz rahat
            //-------------------------------dede boba amma uje rahat versiya
            List<CategoryListDto> categoryListDtos = await _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(x => new CategoryListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentCategory = x.IsMain ? null : x.Parent.Name
                })
                .ToListAsync();
            #endregion

            return Ok(categoryListDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryPostDto categoryPostDto)
        {
            //bu createdir, hesab etki mvc de get etdik, view gaytardig, sonra inputlari doldurdug,
            //create buttonu basdig, geldi posta. Eyni mentignen postu yazmaliyig, bize kategoriya gelir ve biz onu yoxlayib est edirik

            if (categoryPostDto == null) return BadRequest();

            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryPostDto.Name.Trim().ToLower())) return Conflict($"{categoryPostDto.Name} Already exists!");

            if (categoryPostDto.IsMain)
            {
                if (categoryPostDto.Image == null) return BadRequest("Image is required!");
            }
            else
            {
                if (categoryPostDto.ParentId == null) return BadRequest("You must choose Main category!");

                if (!await _context.Categories.AnyAsync(c => c.Id == categoryPostDto.ParentId && c.IsMain)) return BadRequest("Main category is incorrect!");
            }

            categoryPostDto.Name = categoryPostDto.Name.Trim();

            Category category = new Category
            {
                Name = categoryPostDto.Name,
                Image = categoryPostDto.Image,
                ParentId = categoryPostDto.ParentId,
                IsMain = categoryPostDto.IsMain,
                CreatedAt = DateTime.UtcNow.AddHours(4)
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            CategoryGetDto categoryGetDto = new CategoryGetDto
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image,
                ParentId = category.ParentId,
                IsMain = category.IsMain,
                ParentCategory = category.IsMain ? null : category.Parent.Name
            };

            return StatusCode(200, categoryGetDto);
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            //burda uje id lazimdi, cunki bilmeliyik, hansi kategoriyani gormek isteyirik!!!
            //id nin null olmagini yoxlamirig, cunki null gelse, yuxaridaki Gete gedecek, cunki orda id yoxdu, ozu basha dushur!

            CategoryGetDto categoryGetDto = await _context.Categories
                .Where(c => c.Id == id)
                .Select(x => new CategoryGetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    ParentCategory = x.IsMain ? null : x.Parent.Name,
                    Image = x.Image,
                    IsMain = x.IsMain
                })
                .FirstOrDefaultAsync();

            #region dede boba
            //Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            //biz indi .net i aldadacayig

            //yuxaridaki var deye biz bunu etmirik daha, bir bir menimsedmirik
            //CategoryGetDto categoryGetDto = new CategoryGetDto
            //{
            //    Id = category.Id,
            //    Name = category.Name,
            //    ParentId = category.ParentId,
            //    ParentCategory = category.IsMain ? null : category.Parent.Name,
            //    Image = category.Image,
            //    IsMain = category.IsMain
            //};
            #endregion

            return Ok(categoryGetDto);
        }

        [HttpPut]
        [Route("{id?}")]
        public async Task<IActionResult> Put(int? id, CategoryPutDto categoryPutDto)
        {
            if (id == null) return BadRequest("Id is required");

            if (categoryPutDto.Id != id) return BadRequest("Id is not matching to category's Id!");

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (dbCategory == null) return NotFound("Id is wrong!");

            //useri yoxladig, kechirik dbya

            if (categoryPutDto.IsMain)
            {
                if (categoryPutDto.Image == null) return BadRequest("Image is required!");
            }
            else
            {
                if (categoryPutDto.ParentId == null) return BadRequest("You must choose Main category!");

                if (id == categoryPutDto.ParentId) return BadRequest("Id is the same as id of main category!");

                if (!await _context.Categories.AnyAsync(c => c.Id == categoryPutDto.ParentId && c.Id != id && c.IsMain)) return BadRequest("Main category is incorrect!");
            }

            dbCategory.Name = categoryPutDto.Name.Trim();
            dbCategory.ParentId = categoryPutDto.ParentId;
            dbCategory.Image = categoryPutDto.Image;
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
            dbCategory.IsMain = categoryPutDto.IsMain;

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
