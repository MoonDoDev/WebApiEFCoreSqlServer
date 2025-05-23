using Application.Abstractions;
using Domain.Employees;
using FluentValidation;

namespace Application.Employees;

public interface IDeleteCommandHandler : ICommandHandler<DeleteCommand> { }

public sealed class DeleteCommandHandler(
	IEmployeeRepository employeeRepository )
	: IDeleteCommandHandler
{
	public async Task Execute( DeleteCommand employee, CancellationToken cancellationToken = default )
	{
		ArgumentNullException.ThrowIfNull( employee );

		var validator = new DeleteCommandValidator();
		var result = await validator.ValidateAsync( employee, cancellationToken );

		if ( !result.IsValid )
		{
			throw new ValidationException( result.Errors );
		}

		var existingEmployee = await employeeRepository.GetByIdAsync( employee.Id, cancellationToken ) ??
			throw new NotFoundEmployeeException( employee.Id );

		await employeeRepository.DeleteAsync( existingEmployee, cancellationToken );
	}
}

internal class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
	public DeleteCommandValidator()
	{
		RuleFor( x => x.Id )
			.NotEmpty()
			.WithMessage( "Id is required." );
	}
}
