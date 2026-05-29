using DisputePortal.Application.Domain.Models;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<TransactionModel>
{
    public void Configure(EntityTypeBuilder<TransactionModel> builder)
    {
        builder.ToTableName(DbConstants.DataSchema);

        builder.Property(t => t.Amount).HasPostgresNumeric(18, 2).IsRequired();
        builder.Property(t => t.Description).HasPostgresVarChar(500).IsRequired();
        builder.Property(t => t.Reference).HasPostgresVarChar(100).IsRequired();

        builder.HasIndex(t => new { t.CustomerId, t.TransactionDate });
        builder.HasIndex(t => t.AccountId);

        builder.HasOne(t => t.Customer)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CustomerId);

        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId);

        builder.HasOne(t => t.Dispute)
            .WithOne(d => d.Transaction)
            .HasForeignKey<DisputeModel>(d => d.TransactionId);
    }
}
