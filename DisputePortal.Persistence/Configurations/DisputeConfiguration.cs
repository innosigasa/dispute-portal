using DisputePortal.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.ToTable("disputes", "data");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.ReferenceNumber).HasMaxLength(30).IsRequired();
        builder.Property(d => d.Reason).HasConversion<string>().HasMaxLength(50);
        builder.Property(d => d.Status).HasConversion<string>().HasMaxLength(50);
        builder.Property(d => d.Comments).HasMaxLength(1000);
        builder.HasIndex(d => d.ReferenceNumber).IsUnique();
        builder.HasIndex(d => new { d.CustomerId, d.Status });
        builder.HasOne(d => d.Customer).WithMany(c => c.Disputes).HasForeignKey(d => d.CustomerId);
        builder.HasMany(d => d.StatusHistory).WithOne(h => h.Dispute).HasForeignKey(h => h.DisputeId);
        builder.HasMany(d => d.Notifications).WithOne(n => n.Dispute).HasForeignKey(n => n.DisputeId);
    }
}
