using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityMap
{
    public class ProjectMap : AuditableBaseEntityMap<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);

            builder.ToTable("Projects");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasColumnType("TEXT");

            builder.Property(p => p.Date)
                .IsRequired();

            builder.Property(p => p.Latitude)
                .IsRequired();

            builder.Property(p => p.Longitude)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);

            builder.HasMany(p => p.Images)
                .WithOne(pi => pi.Project)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
