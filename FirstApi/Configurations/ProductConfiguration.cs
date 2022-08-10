using FirstApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FirstApi.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired(true).HasMaxLength(10);
            builder.Property(p => p.Description).IsRequired(true).HasMaxLength(100);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p=>p.IsDeleted).HasDefaultValue(false);
            builder.Property(p=>p.UpdatedAt).HasDefaultValue(null);
            builder.Property(p=>p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(p => p.ImgUrl).IsRequired(true);
            builder.Property(p=>p.Count).HasDefaultValue(0);
        }
    }
}
