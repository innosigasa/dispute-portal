using DisputePortal.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions", "data");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(t => t.Description).HasMaxLength(500).IsRequired();
        builder.Property(t => t.Category).HasConversion<string>().HasMaxLength(50);
        builder.Property(t => t.Reference).HasMaxLength(100).IsRequired();
        builder.HasIndex(t => new { t.CustomerId, t.TransactionDate });
        builder.HasIndex(t => t.AccountId);
        builder.HasOne(t => t.Customer).WithMany(c => c.Transactions).HasForeignKey(t => t.CustomerId);
        builder.HasOne(t => t.Account).WithMany(a => a.Transactions).HasForeignKey(t => t.AccountId);
        builder.HasOne(t => t.Dispute).WithOne(d => d.Transaction).HasForeignKey<Dispute>(d => d.TransactionId);
    }
}
