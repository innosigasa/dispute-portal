using DisputePortal.Application.Domain.Models;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<NotificationModel>
{
    public void Configure(EntityTypeBuilder<NotificationModel> builder)
    {
        builder.ToTableName(DbConstants.DataSchema);

        builder.Property(n => n.CreatedAt).HasPostgresTimeStampTz().HasPostgresDefaultCurrentTimeStampTz();
        builder.Property(n => n.Message).HasMaxLength(500).IsRequired();

        builder.HasOne(n => n.Customer)
            .WithMany()
            .HasForeignKey(n => n.CustomerId);
    }
}
