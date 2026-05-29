using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DisputePortal.Persistence;

public class DisputePortalDbContext : DbContext
{
    public DisputePortalDbContext(DbContextOptions<DisputePortalDbContext> options) : base(options)
    {
    }

    public DbSet<AppUserModel> Users => Set<AppUserModel>();
    public DbSet<RefreshTokenModel> RefreshTokens => Set<RefreshTokenModel>();
    public DbSet<CustomerModel> Customers => Set<CustomerModel>();
    public DbSet<BankAccountModel> BankAccounts => Set<BankAccountModel>();
    public DbSet<TransactionModel> Transactions => Set<TransactionModel>();
    public DbSet<DisputeModel> Disputes => Set<DisputeModel>();
    public DbSet<DisputeStatusHistoryModel> DisputeStatusHistories => Set<DisputeStatusHistoryModel>();
    public DbSet<NotificationModel> Notifications => Set<NotificationModel>();
    public DbSet<DisputeReasonCodeModel> DisputeReasons => Set<DisputeReasonCodeModel>();
    public DbSet<TransactionCategoryModel> TransactionCategories => Set<TransactionCategoryModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DisputePortalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
