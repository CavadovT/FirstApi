using FluentValidation;
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
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto> 
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("don't Empty").MaximumLength(10).WithMessage("maximum character 10");
            RuleFor(p => p.Description).NotEmpty().WithMessage("don't Empty").MinimumLength(10).WithMessage("minimum character 10");
            RuleFor(p => p.IsActive).NotEmpty().WithMessage("don't Empty");
            RuleFor(c => c.Photo).NotEmpty().WithMessage("Change the foto for category ");

        }
       
    }
}
