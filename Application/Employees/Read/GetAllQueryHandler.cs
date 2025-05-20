using Application.Abstractions;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees;

public interface IQueryAllHandler : IQueryHandler<IEnumerable<Employee>>
{
	Task<IEnumerable<Employee>> Handle( CancellationToken cancellationToken = default );
}

internal sealed class GetAllQueryHandler(
	IAppDbContext appDbContext )
	: IQueryAllHandler
{
	public async Task<IEnumerable<Employee>> Handle( CancellationToken cancellationToken = default )
	{
		return await appDbContext.Employees.ToListAsync( cancellationToken );
	}
}
