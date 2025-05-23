using Application.Abstractions;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees;

public interface IQueryAllHandler : IQueryHandler<IEnumerable<Employee>> { }

internal sealed class GetAllQueryHandler( 
	IAppDbContext appDbContext ) : IQueryAllHandler
{
	public async Task<IEnumerable<Employee>> Execute( CancellationToken cancellationToken = default )
	{
		return await appDbContext.Employees.ToListAsync( cancellationToken );
	}
}
