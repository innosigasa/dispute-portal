using DisputePortal.Application.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence.Configurations
{
    internal class AccountTypeConfiguration: IEntityTypeConfiguration<AccountTypeModel>
    {
        public void Configure(EntityTypeBuilder<AccountTypeModel> builder)
        {
            builder.ToTableName(DbConstants.DataSchema);

            builder.Property(r => r.Name).HasPostgresVarChar(256).IsRequired();
            builder.Property(r => r.Description).HasPostgresVarChar(512).IsRequired();

            builder.HasIndex(r => r.Name).IsUnique();

            builder.HasData(Enum.GetValues<AccountType>()
                .Select(r => r.ToModel())
                .ToArray());
        }
    }
}
