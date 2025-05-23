using Application.Abstractions;
using Domain.Employees;
using FluentValidation;

namespace Application.Employees;

public interface ICreateCommandHandler : ICommandHandler<CreateCommand, Employee> { }

public sealed class CreateCommandHandler(
	IEmployeeRepository employeeRepository )
	: ICreateCommandHandler
{
	public async Task<Employee> Execute( CreateCommand employee, CancellationToken cancellationToken = default )
	{
		ArgumentNullException.ThrowIfNull( employee );

		var validator = new CreateCommandValidator();
		var result = await validator.ValidateAsync( employee, cancellationToken );

		if ( !result.IsValid )
		{
			throw new ValidationException( result.Errors );
		}

		var employeeEntity = new Employee(
			Guid.NewGuid(),
			employee.Name,
			employee.Position,
			employee.Salary );

		await employeeRepository.AddAsync( employeeEntity, cancellationToken );
		return employeeEntity;
	}
}

internal class CreateCommandValidator : AbstractValidator<CreateCommand>
{
	public CreateCommandValidator()
	{
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
