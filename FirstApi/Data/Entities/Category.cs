namespace FirstApi.Data.Entities

{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
