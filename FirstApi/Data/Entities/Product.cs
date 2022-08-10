
namespace FirstApi.Data.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
    }
}
