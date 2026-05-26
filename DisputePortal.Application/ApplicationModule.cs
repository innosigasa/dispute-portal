using DisputePortal.Application.Feature.Account.Service;
using DisputePortal.Application.Feature.Auth.Service;
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

    public static IServiceCollection RegisterApplicationModule(this IServiceCollection services)
    {
        if (_isModuleAdded) return services;
        _isModuleAdded = true;

        services.AddValidatorsFromAssembly(typeof(ApplicationModule).Assembly);

        // Auth
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Domain services
        services.AddScoped<IBankAccountService, BankAccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IDisputeService, DisputeService>();
        services.AddScoped<IAgentDisputeService, AgentDisputeService>();
        services.AddSingleton<IReferenceNumberGenerator, ReferenceNumberGenerator>();

        // Infrastructure services
        services.AddScoped<ILookupService, LookupService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
