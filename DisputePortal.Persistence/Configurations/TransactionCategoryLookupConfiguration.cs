using DisputePortal.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class TransactionCategoryLookupConfiguration : IEntityTypeConfiguration<TransactionCategoryLookup>
{
    public void Configure(EntityTypeBuilder<TransactionCategoryLookup> builder)
    {
        builder.ToTable("transaction_categories", "static");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Code).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Label).HasMaxLength(200).IsRequired();
        builder.HasIndex(c => c.Code).IsUnique();
    }
}
