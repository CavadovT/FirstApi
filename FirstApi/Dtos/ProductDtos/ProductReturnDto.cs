using FirstApi.Data.Entities;
using System.Collections.Generic;

namespace FirstApi.Dtos.ProductDtos
{
    public class ProductReturnDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Imgurl { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
    }
}
