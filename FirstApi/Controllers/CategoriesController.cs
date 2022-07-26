using AutoMapper;
using FirstApi.Data;
using FirstApi.Data.Entities;
using FirstApi.DTOs.CategoryDTOs;
using FirstApi.Interfaces;
using FirstApi.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Controllers
{
    /// <summary>
    /// Categories Controlelr
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJWTManager _jWTManager;
        /// <summary>
        /// Constructor of Category controller
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="jWTManager"></param>
        public CategoriesController(AppDbContext context, IMapper mapper, IJWTManager jWTManager)
        {
            _context = context;
            _mapper = mapper;
            _jWTManager = jWTManager;
        }

        /// <summary>
        /// Get Categories as list
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get category by posting id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create category
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Get api/categories
        ///     {        
        ///       "name": "New category",
        ///       "parentId": "0",
        ///       "isMain": "true",
        ///       "image": "itisphoto.jpg"        
        ///     }
        /// </remarks>
        /// <param name="categoryPostDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CategoryPostDto categoryPostDto)
        {
            string token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            //bele tokeni goture bilirik request gelende

            string userName = _jWTManager.GetUserNameByToken(token);
            //bu da bize username gaytarir serviceden

            if (categoryPostDto == null) return BadRequest();

            if (categoryPostDto.ParentId != null && !await _context.Categories.AnyAsync(c => c.Id == categoryPostDto.ParentId && c.IsMain)) return BadRequest("Main category is incorrect!");

            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == categoryPostDto.Name.Trim().ToLower())) return Conflict($"{categoryPostDto.Name} Already exists!");

            Category category = _mapper.Map<Category>(categoryPostDto);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            CategoryGetDto categoryGetDto = _mapper.Map<CategoryGetDto>(category);

            return StatusCode(200, categoryGetDto);
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryPutDto"></param>
        /// <returns>Returns updated Category with changed udatedAt and isUpdated properties </returns>
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

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns Category with isDeleted property being true</returns>
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

        /// <summary>
        /// Restore category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns Category with isDeleted property being false</returns>
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
