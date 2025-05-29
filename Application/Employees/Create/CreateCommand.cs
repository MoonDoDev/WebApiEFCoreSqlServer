using System.ComponentModel.DataAnnotations;
using Domain.Employees;

namespace Application.Employees;

public sealed record CreateCommand
{
    [Required( ErrorMessage = "Name is required." )]
    public required string Name { get; init; }

    [Required( ErrorMessage = "Position is required." )]
    public required string Position { get; init; }

    [Required( ErrorMessage = "Salary is required." )]
    [Range( 0, double.MaxValue, ErrorMessage = "Salary must be a positive number." )]
    public required decimal Salary { get; init; }
}

internal static class CreateCommandExtensions
{
    public static Employee MapToEmployee( this CreateCommand command )
    {
        if ( command is not null )
        {
            return new Employee(
                Guid.NewGuid(),
                command.Name,
                command.Position,
                command.Salary );
        }

        throw new ArgumentNullException( nameof( command ) );
    }
}
