using DisputePortal.Application.Domain.Entities;
using DisputePortal.Application.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("bank_accounts", "data");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.AccountNumber).HasMaxLength(50).IsRequired();
        builder.Property(a => a.AccountType).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(a => a.AccountName).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Balance).HasPrecision(18, 2).IsRequired();
        builder.Property(a => a.Currency).HasMaxLength(3).IsRequired().HasDefaultValue("ZAR");
        builder.HasIndex(a => a.AccountNumber).IsUnique();
        builder.HasIndex(a => a.CustomerId);
        builder.HasOne(a => a.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
