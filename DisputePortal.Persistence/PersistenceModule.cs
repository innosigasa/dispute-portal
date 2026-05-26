using DisputePortal.Application.Feature.Account.Persistence;
using DisputePortal.Application.Feature.Dispute.Persistence;
using DisputePortal.Application.Feature.Lookup.Persistence;
using DisputePortal.Application.Feature.Notification.Persistence;
using DisputePortal.Application.Feature.Transaction.Persistence;
using DisputePortal.Application.Feature.User.Persistence;
using DisputePortal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DisputePortal.Persistence;

public static class PersistenceModule
{
    private static bool _isModuleAdded;

    public static IServiceCollection RegisterPersistenceModule(this IServiceCollection services, IConfiguration configuration)
    {
        if (_isModuleAdded) return services;
        _isModuleAdded = true;

        services.AddDbContext<DisputePortalDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IDisputeRepository, DisputeRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILookupRepository, LookupRepository>();

        return services;
    }
}
