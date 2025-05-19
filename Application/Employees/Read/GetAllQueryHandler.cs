using Application.Abstractions;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees;

public sealed class GetAllQueryHandler(
	IAppDbContext appDbContext )
	: IQueryHandler<IEnumerable<Employee>>
{
	public async Task<IEnumerable<Employee>> Handle( CancellationToken cancellationToken = default )
	{
		return await appDbContext.Employees.ToListAsync( cancellationToken );
	}
}
