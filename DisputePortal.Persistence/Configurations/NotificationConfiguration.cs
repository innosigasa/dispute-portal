using DisputePortal.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications", "data");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Message).HasMaxLength(500).IsRequired();
        builder.HasOne(n => n.Customer).WithMany().HasForeignKey(n => n.CustomerId);
    }
}
