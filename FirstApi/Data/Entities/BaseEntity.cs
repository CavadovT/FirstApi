using System;

namespace FirstApi.Data.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
