using System.ComponentModel.DataAnnotations;

namespace Application.Employees;

public sealed record GetByIdQuery
{
	[Required( ErrorMessage = "Id is required." )]
	public required Guid Id { get; init; }
}
