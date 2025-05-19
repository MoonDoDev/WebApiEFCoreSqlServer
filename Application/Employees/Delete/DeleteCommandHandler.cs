using Application.Abstractions;
using Domain.Employees;
using System.ComponentModel.DataAnnotations;

namespace Application.Employees;

public sealed class DeleteCommandHandler(
	IEmployeeRepository employeeRepository )
	: ICommandHandler<DeleteCommand>
{
	public async Task Handle( DeleteCommand employee, CancellationToken cancellationToken = default )
	{
		ArgumentNullException.ThrowIfNull( employee );

		var existingEmployee = await employeeRepository.GetByIdAsync( employee.Id, cancellationToken ) ??
			throw new NotFoundEmployeeException( employee.Id );

		await employeeRepository.DeleteAsync( existingEmployee, cancellationToken );
	}
}

public record DeleteCommand
{
	[Required( ErrorMessage = "Id is required." )]
	public required Guid Id { get; init; }
}