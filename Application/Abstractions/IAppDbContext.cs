using Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Abstractions;

public interface IAppDbContext
{
	DbSet<Employee> Employees { get; set; }

	DatabaseFacade Database { get; }

	Task<int> SaveChangesAsync( CancellationToken cancellationToken = default );
}
