using Application.Abstractions;
using Domain.Employees;
using System.ComponentModel.DataAnnotations;

namespace Application.Employees;

public interface IQueryOneHandler : IQueryHandler<GetByIdQuery, Employee>
{
	Task<Employee> Handle( GetByIdQuery query, CancellationToken cancellationToken = default );
}

internal sealed class GetByIdQueryHandler(
	IEmployeeRepository employeeRepository )
	: IQueryOneHandler
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
