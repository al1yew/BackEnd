using AutoMapper;
using FirstApi.Data;
using FirstApi.Data.Entities;
using FirstApi.DTOs.CategoryDTOs;
using FirstApi.Mappings;
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
        private readonly IMapper _mapper;

        public CategoriesController(AppDbContext context, IMapper mapper = null)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
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
            //List<CategoryListDto> categoryListDtos = await _context.Categories
            //    .Where(c => !c.IsDeleted)
            //    .Select(x => new CategoryListDto
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        ParentCategory = x.IsMain ? null : x.Parent.Name
            //    })
            //    .ToListAsync();
            #endregion

            List<CategoryListDto> categoryListDtos = _mapper.Map<List<CategoryListDto>>(await _context.Categories.Where(c => !c.IsDeleted).ToListAsync());

            return Ok(categoryListDtos);
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            #region dede boba biraz rahat
            //CategoryGetDto categoryGetDto = await _context.Categories
            //               .Where(c => c.Id == id)
            //               .Select(x => new CategoryGetDto
            //               {
            //                   Id = x.Id,
            //                   Name = x.Name,
            //                   ParentId = x.ParentId,
            //                   ParentCategory = x.IsMain ? null : x.Parent.Name,
            //                   Image = x.Image,
            //                   IsMain = x.IsMain
            //               })
            //               .FirstOrDefaultAsync();
            #endregion

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

            CategoryGetDto categoryGetDto = _mapper.Map<CategoryGetDto>(await _context.Categories.FirstOrDefaultAsync(c => c.Id == id));

            return Ok(categoryGetDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryPostDto categoryPostDto)
        {
            if (categoryPostDto == null) return BadRequest();

            if (categoryPostDto.ParentId != null && !await _context.Categories.AnyAsync(c => c.Id == categoryPostDto.ParentId && c.IsMain)) return BadRequest("Main category is incorrect!");

            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryPostDto.Name.Trim().ToLower())) return Conflict($"{categoryPostDto.Name} Already exists!");

            Category category = _mapper.Map<Category>(categoryPostDto);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            CategoryGetDto categoryGetDto = _mapper.Map<CategoryGetDto>(category);

            return StatusCode(200, categoryGetDto);
        }

        [HttpPut]
        [Route("{id?}")]
        public async Task<IActionResult> Put(int? id, CategoryPutDto categoryPutDto)
        {
            //if (id == null) return BadRequest("Id is required");

            if (categoryPutDto.Id != id) return BadRequest("Id is not matching to category's Id!");

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            if (dbCategory == null) return NotFound("Id is wrong!");

            if (categoryPutDto.ParentId != null && !await _context.Categories.AnyAsync(c => c.Id == categoryPutDto.ParentId && c.IsMain)) return BadRequest("Main category is incorrect!");

            //useri yoxladig, kechirik dbya

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

        //restore
        [HttpPut]
        [Route("restore/{id?}")]
        public async Task<IActionResult> Put(int? id)
        {
            if (id == null) return BadRequest("Id is required");

            Category dbCategory = await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);

            if (dbCategory == null) return NotFound("Id is wrong!");

            if (dbCategory.IsMain && dbCategory.Children != null)
            {
                List<Category> children = await _context.Categories.Where(c => c.ParentId == id).ToListAsync();
                //ushaglari varsa onlari da restore etmek lazimdi, bunu daha qisa yazmaq lazimdi amma oxunaqli olsun
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
