using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityMap
{
    public class ProductCategoryMap : AuditableBaseEntityMap<ProductCategory>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            base.Configure(builder);

            builder.ToTable("ProductCategories");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(255);
        }
    }
}
