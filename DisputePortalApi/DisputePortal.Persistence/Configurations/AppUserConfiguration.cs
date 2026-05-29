using DisputePortal.Application.Domain.Models;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUserModel>
{
    public void Configure(EntityTypeBuilder<AppUserModel> builder)
    {
        builder.ToTableName(DbConstants.SecuritySchema);

        builder.Property(u => u.Email).HasPostgresVarChar(256).IsRequired();
        builder.Property(u => u.PasswordHash).HasPostgresText().IsRequired();
        builder.Property(u => u.Role).HasPostgresVarChar(256).IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);
    }
}
