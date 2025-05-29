using Application.Abstractions;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        var connectionString = configuration.GetConnectionString( Constants.DatabaseConnName ) ??
            throw new InvalidOperationException( $"Connection string '{Constants.DatabaseConnName}' is not configured." );

        services.AddDbContext<AppDbContext>( options =>
            options.UseSqlServer( connectionString,
                opt => opt.MigrationsAssembly( "Persistence" ) ) );

        services.AddScoped<IAppDbContext>( provider => provider.GetRequiredService<AppDbContext>() );
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        services.AddHealthChecks().AddSqlServer(
            connectionString,
            "select 1;",
            null,
            "Sql-Server",
            HealthStatus.Unhealthy,
            ["ready"],
            TimeSpan.FromSeconds( 5 ) );

        services.AddMemoryCache();

        return services;
    }
}
