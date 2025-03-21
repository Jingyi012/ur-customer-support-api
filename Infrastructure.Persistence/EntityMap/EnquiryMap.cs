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
    public class EnquiryMap : AuditableBaseEntityMap<Enquiry>
    {
        public override void Configure(EntityTypeBuilder<Enquiry> builder)
        {
            base.Configure(builder);

            builder.ToTable("Enquiries");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

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

            builder.HasMany(e => e.History)
                .WithOne(h => h.Enquiry)
                .HasForeignKey(h => h.EnquiryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
