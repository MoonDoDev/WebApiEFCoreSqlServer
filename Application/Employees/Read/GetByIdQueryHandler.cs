using Application.Abstractions;
using Domain.Employees;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Employees;

public sealed class GetByIdQueryHandler(
	IEmployeeRepository employeeRepository )
	: IQueryHandler<GetByIdQuery, Employee>
{
	public async Task<Employee> Handle( GetByIdQuery query, CancellationToken cancellationToken = default )
	{
		ArgumentNullException.ThrowIfNull( query );

		var employee = await employeeRepository.GetByIdAsync( query.Id, cancellationToken ) ?? 
			throw new NotFoundEmployeeException( query.Id );

		return employee;
	}
}

public record GetByIdQuery
{
	[Required( ErrorMessage = "Id is required." )]
	public required Guid Id { get; init; }
}
