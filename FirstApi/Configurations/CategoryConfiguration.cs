using FirstApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstApi.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.IsDeleted).HasDefaultValue(null);
            builder.Property(c => c.Name).IsRequired(true).HasMaxLength(10);
            builder.Property(c => c.Description).IsRequired(true).HasMaxLength(100);
            builder.Property(c => c.IsDeleted).HasDefaultValue(false);
            builder.Property(c => c.IsActive).HasDefaultValue(true);
            builder.Property(p => p.ImgUrl).IsRequired(true);
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
