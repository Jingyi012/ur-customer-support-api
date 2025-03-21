using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityMap
{
    public class EnquiryHistoryMap : AuditableBaseEntityMap<EnquiryHistory>
    {
        public override void Configure(EntityTypeBuilder<EnquiryHistory> builder)
        {
            base.Configure(builder);

            builder.ToTable("EnquiryHistories");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Type)
                .IsRequired();

            builder.Property(e => e.Message)
                .IsRequired()
                .HasColumnType("TEXT");

            builder.Property(e => e.Status)
                .IsRequired();

            builder.Property(e => e.AssignedTo)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(e => e.Remarks)
                .IsRequired(false)
                .HasColumnType("TEXT");

            // Foreign key to Enquiry
            builder.HasOne(e => e.Enquiry)
                .WithMany(e => e.History)
                .HasForeignKey(e => e.EnquiryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
