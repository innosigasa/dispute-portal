using DisputePortal.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens", "data");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.TokenHash).IsRequired();
        builder.HasIndex(r => new { r.UserId, r.ExpiresAt });
    }
}
