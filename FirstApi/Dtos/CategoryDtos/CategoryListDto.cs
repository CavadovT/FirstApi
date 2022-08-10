using System.Collections.Generic;

namespace FirstApi.Dtos.CategoryDtos
{
    public class CategoryListDto
    {
        public int TotalCount { get; set; }
        public List<CategoryReturnDto> items { get; set; }
    }
}
