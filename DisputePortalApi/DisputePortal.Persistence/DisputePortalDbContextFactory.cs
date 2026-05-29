using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DisputePortal.Persistence
{
    internal class DisputePortalDbContextFactory : IDesignTimeDbContextFactory<DisputePortalDbContext>
    {
        public DisputePortalDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DisputePortalDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            builder.UseNpgsql(GetConnectionString(configuration), opt =>
            {
                opt.MigrationsAssembly(typeof(DisputePortalDbContext).Assembly.FullName)
                    .EnableRetryOnFailure();
            });

            return new DisputePortalDbContext(builder.Options);
        }


        public static string GetConnectionString(IConfiguration configuration)
            => configuration.GetConnectionString(DbConstants.DisputePortalDbKey)
               ?? throw new ArgumentNullException(DbConstants.DisputePortalDbKey);

        public static DbContextOptions<DisputePortalDbContext> GetOptions(IConfiguration configuration)
        {
            var connectionString = GetConnectionString(configuration);
            var optionsBuilder = new DbContextOptionsBuilder<DisputePortalDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }
    }
}
