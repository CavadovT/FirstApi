using AutoMapper;
using AutoMapper.QueryableExtensions;
using FirstApi.Data;
using FirstApi.Data.Entities;
using FirstApi.Dtos.ProductDtos;
using FirstApi.Extentions;
using FirstApi.Helpers;
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
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _maper;


        public ProductController(AppDbContext context, IWebHostEnvironment environment, IMapper maper)
        {
            _context = context;
            _env = environment;
            _maper = maper;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<ProductReturnDto> products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsDeleted == false)
                .ProjectTo<ProductReturnDto>(_maper.ConfigurationProvider
                //p =>

                //new ProductReturnDto
                //{
                //    Name = p.Name,
                //    Price = p.Price,
                //    Description = p.Description,
                //    StockCount = p.Count,
                //    IsActive = p.IsActive,
                //    CategoryName = p.Category.Name,
                //    Imgurl = Path.Combine(Request.Path,"/", "img/", p.ImgUrl),
                //}
                ).AsQueryable().AsNoTracking().ToListAsync();


            ListDto<ProductReturnDto> productListDto = new ListDto<ProductReturnDto>()
            {
                items = products,
                TotalCount = products.Count,
            };

            return StatusCode(200, productListDto);
        }
        /// <summary>
        /// get one product for id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null) RedirectToAction("GetAll");

            Product dbProd = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (dbProd == null) return StatusCode(404, "Product Not Found");

            ProductReturnDto productReturnDto = new ProductReturnDto()
            {
                Name = dbProd.Name,
                Description = dbProd.Description,
                CategoryName = dbProd.Category.Name,
                Price = dbProd.Price,
                Imgurl = Path.Combine(Request.Path,"/", "img/", dbProd.ImgUrl),
                StockCount = dbProd.Count,
                IsActive = dbProd.IsActive,
            };

            return StatusCode(200, productReturnDto);
        }
        /// <summary>
        /// Create action
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto pcdto)
        {
            bool IsExistProd = _context.Products.Any(c => c.Name.Trim().ToLower() == pcdto.Name.Trim().ToLower());
            if (IsExistProd) return StatusCode(600, "this category already exist");
            if (pcdto.Photo == null)
            {
                return StatusCode(601, "Select photo with product");
            }
            if (!pcdto.Photo.IsImage())
            {
                return StatusCode(602, "only photo");
            }
            if (pcdto.Photo.ValidSize(200))
            {
                return StatusCode(603, "file Oversize");
            }
            Product newProduct = new Product()
            {
                Name = pcdto.Name,
                Description = pcdto.Description,
                IsActive = pcdto.IsActive,
                Price = pcdto.Price,
                ImgUrl = pcdto.Photo.SaveImage(_env, "img"),
                CreatedAt = DateTime.Now,
                Count = pcdto.StockCount,
                CategoryId = pcdto.CategoryId,
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return StatusCode(201, "Product Created");
        }
        /// <summary>
        /// update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductUpdateDto productUpdateDto)
        {
            Product dbProd = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (dbProd == null) return StatusCode(404, "product not found");

            bool dbProductNameExist = await _context.Products.AnyAsync(p => p.Name.Trim().ToLower() == productUpdateDto.Name.Trim().ToLower() && p.Id != dbProd.Id);
            if (dbProductNameExist)
            {
                return StatusCode(603, "this product already exist");
            }
            if (productUpdateDto.Photo != null)
            {
                if (!productUpdateDto.Photo.IsImage())
                {
                    return StatusCode(602, "only photo");
                }
                if (productUpdateDto.Photo.ValidSize(200))
                {
                    return StatusCode(603, "file Oversize");
                }
                string path = Path.Combine(_env.WebRootPath, "img", dbProd.ImgUrl);
                Helpers.Helpers.DeleteImage(path);
                dbProd.ImgUrl = productUpdateDto.Photo.SaveImage(_env, "img");
            }
            dbProd.Price = productUpdateDto.Price;
            dbProd.Name = productUpdateDto.Name;
            dbProd.IsActive = productUpdateDto.IsActive;
            dbProd.Description = productUpdateDto.Description;
            dbProd.CategoryId = productUpdateDto.CategroyId;
            dbProd.Count = productUpdateDto.Count;
            dbProd.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok($"id: {dbProd.Id}, product Updated");
        }

        /// <summary>
        /// Edit price of products
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditIsActive(int id, decimal price)
        {
            Product dbProd = _context.Products.FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
            if (dbProd == null) return NotFound();
            dbProd.Price = price;
            await _context.SaveChangesAsync();
            return StatusCode(200, $"id:{dbProd.Id} isActive editing newprice= {price}");
        }
        /// <summary>
        /// Delete Product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product dbProd = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (dbProd == null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "img", dbProd.ImgUrl);

            Helpers.Helpers.DeleteImage(path);

            dbProd.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok($"Id:{id} product Deleted");

        }

    }
}
