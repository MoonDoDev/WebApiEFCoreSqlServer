using Application.Abstractions;
using Domain.Employees;
using System.ComponentModel.DataAnnotations;

namespace Application.Employees;

public sealed class UpdateCommandHandler(
	IEmployeeRepository employeeRepository )
	: ICommandHandler<UpdateCommand, Employee>
{
	public async Task<Employee> Handle( UpdateCommand employee, CancellationToken cancellationToken = default )
	{
		ArgumentNullException.ThrowIfNull( employee );

		var existingEmployee = await employeeRepository.GetByIdAsync( employee.Id, cancellationToken ) ?? 
			throw new NotFoundEmployeeException( employee.Id );

		existingEmployee.Update( employee.Name, employee.Position, employee.Salary );
		await employeeRepository.UpdateAsync( existingEmployee, cancellationToken );
		return existingEmployee;
	}
}

public record UpdateCommand
{
	[Required( ErrorMessage = "Id is required." )]
	public Guid Id { get; init; }

	[Required( ErrorMessage = "Name is required." )]
	public required string Name { get; init; }

	[Required( ErrorMessage = "Position is required." )]
	public required string Position { get; init; }

	[Required( ErrorMessage = "Salary is required." )]
	[Range( 0, double.MaxValue, ErrorMessage = "Salary must be a positive number." )]
	public required decimal Salary { get; init; }
}