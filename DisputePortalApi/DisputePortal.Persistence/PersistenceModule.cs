using DisputePortal.Application.Feature.Account.Persistence;
using DisputePortal.Application.Feature.Dispute.Persistence;
using DisputePortal.Application.Feature.Lookup.Persistence;
using DisputePortal.Application.Feature.Notification.Persistence;
using DisputePortal.Application.Feature.Transaction.Persistence;
using DisputePortal.Application.Feature.User.Persistence;
using DisputePortal.Persistence.Repositories;
using DisputePortal.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

        services.AddDbContext<DisputePortalDbContext>((sp, options) =>
        {
            var connectionString = DisputePortalDbContextFactory.GetConnectionString(configuration);
            options.UseNpgsql(connectionString, npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(DisputePortalDbContext).Assembly.FullName)
                        .EnableRetryOnFailure();
                })
                .EnableSensitiveDataLogging();

            options.ConfigureWarnings(warnings =>
            {
                warnings.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning);
            });
        });

        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IDisputeRepository, DisputeRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILookupRepository, LookupRepository>();

        ApplyMigration(services, configuration);

        return services;
    }

    private static void ApplyMigration(IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimeStampBehavior", true);
        var context = new DisputePortalDbContext(DisputePortalDbContextFactory.GetOptions(configuration));
        context.Database.Migrate();
        DataSeeder.SeedAsync(context, services,configuration).GetAwaiter().GetResult();
    }
}
