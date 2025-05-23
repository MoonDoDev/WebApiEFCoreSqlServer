using Application.Abstractions;
using Domain.Employees;
using FluentValidation;

namespace Application.Employees;

public interface IQueryOneHandler : IQueryHandler<GetByIdQuery, Employee> { }

public sealed class GetByIdQueryHandler(
	IEmployeeRepository employeeRepository ) : IQueryOneHandler
{
	public async Task<Employee> Execute( GetByIdQuery query, CancellationToken cancellationToken = default )
	{
		ArgumentNullException.ThrowIfNull( query );

		var validator = new GetByIdQueryValidator();
		var result = await validator.ValidateAsync( query, cancellationToken );

		if ( !result.IsValid )
		{
			throw new ValidationException( result.Errors );
		}

		var employee = await employeeRepository.GetByIdAsync( query.Id, cancellationToken ) ??
			throw new NotFoundEmployeeException( query.Id );

		return employee;
	}
}

internal class GetByIdQueryValidator: AbstractValidator<GetByIdQuery>
{
	public GetByIdQueryValidator()
	{
		RuleFor( x => x.Id )
			.NotEmpty()
			.WithMessage( "Id is required." );
	}
}
