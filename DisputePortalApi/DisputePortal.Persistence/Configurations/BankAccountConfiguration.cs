using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Domain.Models;
using DisputePortal.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccountModel>
{
    public void Configure(EntityTypeBuilder<BankAccountModel> builder)
    {
        builder.ToTableName(DbConstants.DataSchema);

        builder.Property(a => a.AccountNumber).HasPostgresVarChar(50).IsRequired();
        builder.Property(a => a.AccountName).HasPostgresVarChar(100).IsRequired();
        builder.Property(a => a.Balance).HasPrecision(18, 2).IsRequired();
        builder.Property(a => a.Currency).HasPostgresChar(3).HasDefaultValue(AppConstants.CurrencyZAR);
        
        builder.HasIndex(a => a.AccountNumber).IsUnique();
        builder.HasIndex(a => a.CustomerId);

        builder.HasOne(a => a.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
