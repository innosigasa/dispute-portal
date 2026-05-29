using DisputePortal.Application.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence.Configurations
{
    internal class DisputeStatusConfiguration: IEntityTypeConfiguration<DisputeStatusModel>
    {
        public void Configure(EntityTypeBuilder<DisputeStatusModel> builder)
        {
            builder.ToTableName(DbConstants.StaticSchema);

            builder.Property(r => r.Name).HasPostgresVarChar(256).IsRequired();
            builder.Property(r => r.Description).HasPostgresVarChar(512).IsRequired();

            builder.HasIndex(r => r.Name).IsUnique();

            builder.HasData(Enum.GetValues<DisputeStatus>()
                .Select(r => r.ToModel())
                .ToArray());
        }
    }
}
