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
    public class NewsMap : AuditableBaseEntityMap<News>
    {
        public override void Configure(EntityTypeBuilder<News> builder)
        {
            base.Configure(builder);

            builder.ToTable("News");

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.Description)
                .IsRequired(false)
                .HasColumnType("TEXT");

            builder.Property(n => n.Date)
                .IsRequired();

            builder.Property(n => n.IsActive)
                .HasDefaultValue(true);

            builder.HasMany(n => n.Images)
                .WithOne(ni => ni.News)
                .HasForeignKey(ni => ni.NewsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
