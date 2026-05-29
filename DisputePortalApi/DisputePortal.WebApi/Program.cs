using DisputePortal.Application;
using DisputePortal.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using DisputePortal.Application.Feature.Auth.Settings;
using DisputePortal.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

ConfigureSettings(builder.Configuration);
ConfigureLogging(builder.Configuration);
RegisterServices(builder.Services, builder.Configuration);

builder.Host.UseSerilog();

var app = builder.Build();
await ConfigureAppAsync(app);

app.Run();

void ConfigureSettings(IConfigurationBuilder configuration)
{
    configuration
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}

void ConfigureLogging(IConfiguration configuration)
{
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    var passwordHashSettings = configuration.GetSection(PasswordHashSettings.SectionName).Get<PasswordHashSettings>()
        ?? throw new InvalidOperationException("PasswordHashSettings is not configured.");
    var jwtBearerSettings = configuration.GetSection(JwtBearerSettings.SectionName).Get<JwtBearerSettings>()
        ?? throw new InvalidOperationException("JwtBearerSettings is not configured.");

    // Application & Persistence layers
    services.RegisterApplicationModule(jwtBearerSettings, passwordHashSettings)
        .RegisterPersistenceModule(configuration);

    services.AddControllers()
        .AddJsonOptions(opts =>
            opts.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
    services.AddEndpointsApiExplorer();

    // Swagger
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dispute Portal API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                []
            }
        });
        var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
    });

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtBearerSettings.Issuer,
                ValidAudience = jwtBearerSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerSettings.SecretKey))
            };
        });

    services.AddAuthorization(opts =>
    {
        opts.AddPolicy("CustomerOnly", p => p.RequireRole("customer"));
        opts.AddPolicy("AgentOnly", p => p.RequireRole("agent"));
    });

    // CORS
    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    services.AddCors(opts =>
        opts.AddDefaultPolicy(p => p.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

    services.AddHttpContextAccessor();
    services.AddExceptionHandler<GlobalExceptionHandler>();
    services.AddProblemDetails();
}

async Task ConfigureAppAsync(WebApplication webApp)
{
    if (webApp.Environment.IsDevelopment())
    {
        webApp.UseSwagger();
        webApp.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dispute Portal API v1"));
    }

    webApp.UseSerilogRequestLogging();
    webApp.UseExceptionHandler();
    webApp.UseCors();
    webApp.UseAuthentication();
    webApp.UseAuthorization();
    webApp.MapControllers();
}
