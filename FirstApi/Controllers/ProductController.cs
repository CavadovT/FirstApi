using FirstApi.Data;
using FirstApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return StatusCode(200, await _context.Products.Where(p => p.IsActive).ToListAsync());
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
                .Where(p => p.IsActive)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (dbProd == null) return StatusCode(404, "Product Not Found");

            return StatusCode(200, dbProd);
        }
        /// <summary>
        /// Create action
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return StatusCode(201, product);
        }
        /// <summary>
        /// update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            Product dbProd = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (dbProd == null) return StatusCode(404, "product not found");
            dbProd.Price = product.Price;
            dbProd.Name = product.Name;
            dbProd.IsActive = product.IsActive;
            await _context.SaveChangesAsync();
            return Ok($"id: {dbProd.Id}, product Updated");
        }
        /// <summary>
        /// Edit Is Active prop of products
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditIsActive(int id, bool isActive)
        {
            Product dbProd = _context.Products.FirstOrDefault(p => p.Id == id);
            if (dbProd == null) return NotFound();
            dbProd.IsActive = isActive;
            await _context.SaveChangesAsync();
            return StatusCode(200, $"id:{dbProd.Id} isActive editing {isActive}");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product dbProd = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(dbProd==null) return NotFound();
            _context.Products.Remove(dbProd);
            await _context.SaveChangesAsync();
            return Ok($"{id} product Deleted");

        }

    }
}
