using Application.Abstractions;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class AppDbContext( DbContextOptions options ) : DbContext( options ), IAppDbContext
{
	public DbSet<Employee> Employees { get; set; }

	protected override void OnModelCreating( ModelBuilder modelBuilder )
	{
		modelBuilder.Entity<Employee>( entity =>
		{
			entity.HasKey( e => e.Id );
			entity.Property( e => e.Name ).IsRequired().HasMaxLength( 100 );
			entity.Property( e => e.Position ).IsRequired().HasMaxLength( 50 );
			entity.Property( e => e.Salary ).IsRequired().HasColumnType( "decimal(18,2)" );
		} );
	}

	public override async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
	{
		return await base.SaveChangesAsync( cancellationToken );
	}
}
