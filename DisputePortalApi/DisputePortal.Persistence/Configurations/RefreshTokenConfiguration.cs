using DisputePortal.Application.Domain.Models;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenModel>
{
    public void Configure(EntityTypeBuilder<RefreshTokenModel> builder)
    {
        builder.ToTableName(DbConstants.SecuritySchema);

        builder.Property(r => r.TokenHash).HasPostgresText().IsRequired();

        builder.HasIndex(r => new { r.UserId, r.ExpiresAt });
    }
}
