using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FirstApi.Dtos.ProductDtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockCount { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
    }
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(p => p.Photo).NotEmpty().WithMessage("Change the foto for product");
            RuleFor(p => p.Name).NotEmpty().WithMessage("don't Empty").MaximumLength(10).WithMessage("maximum character 10");
            RuleFor(p => p.Description).NotEmpty().WithMessage("don't Empty").MinimumLength(10).WithMessage("minimum character 10");
            RuleFor(p => p.IsActive).NotEmpty().WithMessage("don't Empty");
            RuleFor(p => p.CategoryId).NotNull().WithMessage("Enter the CategoryId");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("greater than 0");
            RuleFor(p => p.StockCount).GreaterThanOrEqualTo(0).WithMessage("say sifirdan asagi ola bilmez");
            RuleFor(p => p).Custom((p, context) => 
            {
                if (p.Price<p.DiscountPrice||p.Price<0)
                {
                    context.AddFailure("Price", "price discount priceden ve sifirdan kicik ola bilmez");
                }
            });


        }

    }
}

