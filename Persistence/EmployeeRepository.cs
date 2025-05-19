using Application.Abstractions;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class EmployeeRepository(
	IAppDbContext appDbContext )
	: IEmployeeRepository
{
	private readonly IAppDbContext _appDbContext = appDbContext ??
		throw new ArgumentNullException( nameof( appDbContext ) );

	public async Task AddAsync( Employee employee, CancellationToken cancellationToken )
	{
		_appDbContext.Employees.Add( employee );
		await _appDbContext.SaveChangesAsync( cancellationToken );
	}

	public async Task DeleteAsync( Employee employee, CancellationToken cancellationToken )
	{
		_appDbContext.Employees.Remove( employee );
		await _appDbContext.SaveChangesAsync( cancellationToken );
	}

	public async Task<Employee?> GetByIdAsync( Guid id, CancellationToken cancellationToken )
	{
		return await _appDbContext.Employees.SingleOrDefaultAsync( e => e.Id == id, cancellationToken );
	}

	public async Task UpdateAsync( Employee employee, CancellationToken cancellationToken )
	{
		_appDbContext.Employees.Update( employee );
		await _appDbContext.SaveChangesAsync( cancellationToken );
	}
}
