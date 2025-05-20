using Application.Abstractions;
using Domain.Employees;
using System.ComponentModel.DataAnnotations;

namespace Application.Employees;

public interface ICreateCommandHandler : ICommandHandler<CreateCommand, Employee>
{
	Task<Employee> Handle( CreateCommand command, CancellationToken cancellationToken = default );
}

internal sealed class CreateCommandHandler(
	IEmployeeRepository employeeRepository )
	: ICreateCommandHandler
{
	public async Task<Employee> Handle( CreateCommand employee, CancellationToken cancellationToken = default )
	{
		ArgumentNullException.ThrowIfNull( employee );

		var employeeEntity = new Employee(
			Guid.NewGuid(),
			employee.Name,
			employee.Position,
			employee.Salary );

		await employeeRepository.AddAsync( employeeEntity, cancellationToken );
		return employeeEntity;
	}
}

public record CreateCommand
{
	[Required( ErrorMessage = "Name is required." )]
	public required string Name { get; init; }

	[Required( ErrorMessage = "Position is required." )]
	public required string Position { get; init; }

	[Required( ErrorMessage = "Salary is required." )]
	[Range( 0, double.MaxValue, ErrorMessage = "Salary must be a positive number." )]
	public required decimal Salary { get; init; }
}