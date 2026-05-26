using DisputePortal.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence;

public class DisputePortalDbContext(DbContextOptions<DisputePortalDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Dispute> Disputes => Set<Dispute>();
    public DbSet<DisputeStatusHistory> DisputeStatusHistories => Set<DisputeStatusHistory>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<DisputeReason> DisputeReasons => Set<DisputeReason>();
    public DbSet<TransactionCategoryLookup> TransactionCategories => Set<TransactionCategoryLookup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DisputePortalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
