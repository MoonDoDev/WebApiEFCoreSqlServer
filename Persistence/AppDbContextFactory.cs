using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;

internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext( string[] args )
	{
		var path = AppDomain.CurrentDomain.BaseDirectory;

		var builder = new ConfigurationBuilder()
			.SetBasePath( path )
			.AddJsonFile( Commons.SETTINGS_FILE_NAME );

		var config = builder.Build();
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

		optionsBuilder.UseSqlServer(
			config.GetConnectionString( Commons.DB_CONNECTION_NAME ) );
		return new AppDbContext( optionsBuilder.Options );
	}
}
