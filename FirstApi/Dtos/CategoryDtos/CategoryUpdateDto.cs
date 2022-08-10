using Microsoft.AspNetCore.Http;

namespace FirstApi.Dtos.CategoryDtos
{
    public class CategoryUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public bool IsActive { get; set; }

    }
}
