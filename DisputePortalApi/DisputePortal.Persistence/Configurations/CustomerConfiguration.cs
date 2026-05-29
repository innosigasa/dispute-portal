using DisputePortal.Application.Domain.Models;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerModel>
{
    public void Configure(EntityTypeBuilder<CustomerModel> builder)
    {
        builder.ToTableName(DbConstants.DataSchema);

        builder.Property(c => c.FullName).HasPostgresVarChar(256).IsRequired();
        builder.Property(c => c.Email).HasPostgresVarChar(256).IsRequired();
        builder.Property(c => c.IdentityNumber).HasPostgresVarChar(50).IsRequired();

        builder.HasIndex(c => c.Email).IsUnique();
    }
}
