using Microsoft.AspNetCore.Http;

namespace FirstApi.Dtos.ProductDtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public bool IsActive { get; set; }
    }
}
