using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;

internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext( string[] args )
	{
		string path = AppDomain.CurrentDomain.BaseDirectory;

		var builder = new ConfigurationBuilder()
			.SetBasePath( path )
			.AddJsonFile( "appsettings.json" );

		var config = builder.Build();
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

		optionsBuilder.UseSqlServer(
			config.GetConnectionString( DependencyInjection.DBConnectionName ) );
		return new AppDbContext( optionsBuilder.Options );
	}
}