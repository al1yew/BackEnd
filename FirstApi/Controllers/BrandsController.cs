using AutoMapper;
using FirstApi.Data;
using FirstApi.Data.Entities;
using FirstApi.DTOs.BrandDTOs;
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
    public class BrandsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BrandsController(IMapper mapper, AppDbContext context = null)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<BrandListDto> brandGetDto = _mapper.Map<List<BrandListDto>>(await _context.Brands.Where(b => !b.IsDeleted).ToListAsync());

            return Ok(brandGetDto);
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            BrandGetDto brandGetDto = _mapper.Map<BrandGetDto>(await _context.Brands.FirstOrDefaultAsync(b => b.Id == id));

            return Ok(brandGetDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BrandPostDto brandPostDto)
        {
            if (brandPostDto == null) return BadRequest();

            if (await _context.Brands.AnyAsync(x => x.Name.ToLower() == brandPostDto.Name.Trim().ToLower())) return BadRequest($"Brand {brandPostDto.Name} already exists!");

            Brand brand = _mapper.Map<Brand>(brandPostDto);

            brand.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            BrandGetDto brandGetDto = _mapper.Map<BrandGetDto>(brand);

            return Ok(brandGetDto);
        }

        [HttpPut]
        [Route("{id?}")]
        public async Task<IActionResult> Put(int? id, BrandPutDto brandPutDto)
        {
            if (brandPutDto == null) return BadRequest();

            if (id != brandPutDto.Id) return BadRequest("Id is not matched to selected brand's Id!");
            //ozu ozunu update etmesin + eyni adda yaradmasin
            if (await _context.Brands.AnyAsync(x => x.Name.ToLower() == brandPutDto.Name.Trim().ToLower())) return BadRequest($"Brand {brandPutDto.Name} already exists!");

            Brand dbBrand = await _context.Brands.Where(b => !b.IsDeleted).FirstOrDefaultAsync(b => b.Id == id);

            if (dbBrand == null) return NotFound("BRand is not found");
            //bu user terefden gelen melumatlardir!!! indni ise kechirik databazaya

            dbBrand.Name = brandPutDto.Name.Trim();
            dbBrand.IsUpdated = true;
            dbBrand.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Content("Updated!");
        }

        [HttpDelete]
        [Route("{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest("Id is required!");

            Brand dbBrand = await _context.Brands.Where(b => !b.IsDeleted).FirstOrDefaultAsync(b => b.Id == id);

            if (dbBrand == null) return NotFound("Brand is not found!");

            dbBrand.IsDeleted = true;
            dbBrand.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Content("Deleted");
        }

        //metodlari yazanda route bildirmek lazimdi!!
        [HttpPut]
        [Route("restore/{id?}")]
        public async Task<IActionResult> Put(int? id)
        {
            if (id == null) return BadRequest("Id is required!");

            Brand dbBrand = await _context.Brands.Where(b => b.IsDeleted).FirstOrDefaultAsync(b => b.Id == id);

            if (dbBrand == null) return NotFound("Brand is not found!");

            dbBrand.IsDeleted = false;
            dbBrand.DeletedAt = null;

            await _context.SaveChangesAsync();

            return Content("Restored");
        }
    }
}
