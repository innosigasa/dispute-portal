using DisputePortal.Application.Feature.Account.Service;
using DisputePortal.Application.Feature.Auth.Service;
using DisputePortal.Application.Feature.Auth.Settings;
using DisputePortal.Application.Feature.Dispute.Service;
using DisputePortal.Application.Feature.Lookup.Service;
using DisputePortal.Application.Feature.Notification.Service;
using DisputePortal.Application.Feature.Transaction.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DisputePortal.Application;

public static class ApplicationModule
{
    private static bool _isModuleAdded;

    public static IServiceCollection RegisterApplicationModule(this IServiceCollection services, 
        JwtBearerSettings jwtBearerSettings, 
        PasswordHashSettings passwordHashSettings)
    {
        if (_isModuleAdded) return services;
        _isModuleAdded = true;

        services.AddValidatorsFromAssembly(typeof(ApplicationModule).Assembly);

        // Auth
        services.AddSingleton(jwtBearerSettings)
            .AddSingleton(passwordHashSettings)
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<JwtTokenService>()
            .AddScoped<IPasswordHasher, PasswordHasher>();

        // Domain services
        services.AddScoped<IBankAccountService, BankAccountService>()
            .AddScoped<ITransactionService, TransactionService>()
            .AddScoped<IDisputeService, DisputeService>()
            .AddScoped<IAgentDisputeService, AgentDisputeService>()
            .AddSingleton<IReferenceNumberGenerator, ReferenceNumberGenerator>();

        // Infrastructure services
        services.AddScoped<ILookupService, LookupService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
