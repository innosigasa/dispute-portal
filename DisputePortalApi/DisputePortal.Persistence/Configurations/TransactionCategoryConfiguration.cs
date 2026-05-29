using DisputePortal.Application.Domain.Enums;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class TransactionCategoryConfiguration : IEntityTypeConfiguration<TransactionCategoryModel>
{
    public void Configure(EntityTypeBuilder<TransactionCategoryModel> builder)
    {
        builder.ToTableName(DbConstants.StaticSchema);

        builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(512).IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasData(Enum.GetValues<TransactionCategory>()
            .Select(c => c.ToModel())
            .ToArray());
    }
}
