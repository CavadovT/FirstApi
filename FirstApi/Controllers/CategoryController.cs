using FirstApi.Data;
using FirstApi.Data.Entities;
using FirstApi.Dtos.CategoryDtos;
using FirstApi.Extentions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _env = environment;
        }
        /// <summary>
        /// Get All Method for Category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<CategoryReturnDto> categories = await _context.Categories
                .Where(c => c.IsDeleted == false)
                .Select(c => new CategoryReturnDto
                {
                    Name = c.Name,
                    Description = c.Description,
                    ImgUrl = c.ImgUrl,
                    IsActive = c.IsActive,

                })
                .AsQueryable().ToListAsync();
            CategoryListDto listcategory = new CategoryListDto()
            {
                items = categories,
                TotalCount = categories.Count
            };
            return Ok(listcategory);
        }
        /// <summary>
        /// Get one category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryReturn"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int? id)
        {
            if (id == null) RedirectToAction("GetAll");

            Category dbCategory = await _context.Categories
                .Where(c => c.IsDeleted == false)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (dbCategory == null) return StatusCode(404, "Category Not Found");

            CategoryReturnDto categoryReturnDto = new CategoryReturnDto()
            {
                Name = dbCategory.Name,
                Description = dbCategory.Description,
                IsActive = dbCategory.IsActive,
                ImgUrl = dbCategory.ImgUrl,
            };

            return StatusCode(200, categoryReturnDto);
        }
        /// <summary>
        /// Create Category
        /// </summary>
        /// <param name="categoryCreateDto"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto categoryCreateDto)
        {
            bool IsExistCat = _context.Categories.Any(c => c.Name.Trim().ToLower() == categoryCreateDto.Name.Trim().ToLower());
            if (IsExistCat) return StatusCode(600, "this category already exist");
            if (categoryCreateDto.Photo == null)
            {
                return StatusCode(601, "Select photo with category");
            }
            if (!categoryCreateDto.Photo.IsImage())
            {
                return StatusCode(602, "only photo");
            }
            if (categoryCreateDto.Photo.ValidSize(200))
            {
                return StatusCode(603, "file Oversize");
            }
            Category newCategory = new Category()
            {
                Name = categoryCreateDto.Name,
                Description = categoryCreateDto.Description,
                IsActive = categoryCreateDto.IsActive,
                ImgUrl = categoryCreateDto.Photo.SaveImage(_env, "img"),
                CreatedAt = DateTime.Now,
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return StatusCode(201, "Category Created");
        }
        /// <summary>
        /// Update Category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto categoryUpdate)
        {
            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (dbCategory == null) return StatusCode(404, "category not found");
            
            if (categoryUpdate.Photo == null)
            {
                dbCategory.ImgUrl = dbCategory.ImgUrl;
            }
            else
            {
                Category dbCategoryName = await _context.Categories.FirstOrDefaultAsync(p => p.Name.Trim().ToLower() == categoryUpdate.Name.Trim().ToLower());
                if (dbCategoryName != null)
                {
                    if (dbCategoryName.Name.Trim().ToLower() != dbCategory.Name.Trim().ToLower())
                    {
                        return StatusCode(603, "this product already exist");
                    }
                }
                if (!categoryUpdate.Photo.IsImage())
                {
                    return StatusCode(602, "only photo");
                }
                if (categoryUpdate.Photo.ValidSize(200))
                {
                    return StatusCode(603, "file Oversize");
                }
                dbCategory.ImgUrl = categoryUpdate.Photo.SaveImage(_env, "img");
            }
           
            dbCategory.Name = categoryUpdate.Name;
            dbCategory.IsActive = categoryUpdate.IsActive;
            dbCategory.Description = categoryUpdate.Description;
            dbCategory.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok($"id:, category Updated");
        }
        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (dbCategory == null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "img", dbCategory.ImgUrl);

            Helpers.Helpers.DeleteImage(path);

            dbCategory.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok($"Id:{id} category Deleted");

        }
    }
}
