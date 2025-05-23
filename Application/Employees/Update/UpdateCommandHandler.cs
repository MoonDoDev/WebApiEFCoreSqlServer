using Application.Abstractions;
using Domain.Employees;
using FluentValidation;

namespace Application.Employees;

public interface IUpdateCommandHandler : ICommandHandler<UpdateCommand, Employee> { }

public sealed class UpdateCommandHandler(
	IEmployeeRepository employeeRepository )
	: IUpdateCommandHandler
{
	public async Task<Employee> Execute( UpdateCommand employee, CancellationToken cancellationToken = default )
	{
		ArgumentNullException.ThrowIfNull( employee );

		var validator = new UpdateCommandValidator();
		var result = await validator.ValidateAsync( employee, cancellationToken );

		if ( !result.IsValid )
		{
			throw new ValidationException( result.Errors );
		}

		var existingEmployee = await employeeRepository.GetByIdAsync( employee.Id, cancellationToken ) ??
			throw new NotFoundEmployeeException( employee.Id );

		existingEmployee.Update( employee.Name, employee.Position, employee.Salary );
		await employeeRepository.UpdateAsync( existingEmployee, cancellationToken );
		return existingEmployee;
	}
}

internal class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
	public UpdateCommandValidator()
	{
		RuleFor( x => x.Id )
			.NotEmpty()
			.WithMessage( "Id is required." );
		RuleFor( x => x.Name )
			.NotEmpty()
			.MinimumLength( 3 )
			.WithMessage( "Name is required." );
		RuleFor( x => x.Position )
			.NotEmpty()
			.MinimumLength( 3 )
			.WithMessage( "Position is required." );
		RuleFor( x => x.Salary )
			.GreaterThan( 0 )
			.WithMessage( "Salary must be a positive number." );
	}
}
