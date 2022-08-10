using Microsoft.AspNetCore.Http;
using System;

namespace FirstApi.Dtos.CategoryDtos
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public bool IsActive { get; set; }
       
    }
}
