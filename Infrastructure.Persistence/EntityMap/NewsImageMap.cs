using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityMap
{
    public class NewsImageMap : AuditableBaseEntityMap<NewsImage>
    {
        public override void Configure(EntityTypeBuilder<NewsImage> builder)
        {
            base.Configure(builder);

            builder.ToTable("NewsImages");

            builder.Property(p => p.ImageUrl)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(pi => pi.Order)
                .IsRequired();

            builder.HasOne(pi => pi.News)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.NewsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
