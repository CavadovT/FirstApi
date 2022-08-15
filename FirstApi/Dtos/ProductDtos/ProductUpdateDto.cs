using Microsoft.AspNetCore.Http;

namespace FirstApi.Dtos.ProductDtos
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategroyId { get; set; }
        public IFormFile Photo { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
    }
}
