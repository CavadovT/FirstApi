using System.Collections.Generic;

namespace FirstApi.Dtos.ProductDtos
{
    public class ProductListDto
    {
        public int TotalCount { get; set; }
        public List<ProductReturnDto> items { get; set; }
    }
}
