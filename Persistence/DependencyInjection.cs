using Application.Abstractions;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public const string DBConnectionName = "DefaultConnection";

    public static IServiceCollection AddPersistence( this IServiceCollection services, IConfiguration configuration )
    {
        services.AddDbContext<AppDbContext>( options =>
            options.UseSqlServer( configuration.GetConnectionString( DBConnectionName ),
                opt => opt.MigrationsAssembly( "Persistence" ) ) );

        services.AddScoped<IAppDbContext>( provider => provider.GetRequiredService<AppDbContext>() );
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddMemoryCache();

        return services;
    }
}
