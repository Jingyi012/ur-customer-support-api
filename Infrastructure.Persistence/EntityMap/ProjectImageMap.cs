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
    public class ProjectImageMap : AuditableBaseEntityMap<ProjectImage>
    {
        public override void Configure(EntityTypeBuilder<ProjectImage> builder)
        {
            base.Configure(builder);

            builder.ToTable("ProjectImages");

            builder.Property(p => p.ImageUrl)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(pi => pi.Order)
                .IsRequired();

            builder.HasOne(pi => pi.Project)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
