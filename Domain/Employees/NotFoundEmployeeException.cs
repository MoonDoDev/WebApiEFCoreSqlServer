namespace Domain.Employees;

public class NotFoundEmployeeException : Exception
{
	public NotFoundEmployeeException( Guid id ) :
		base( $"Employee with ID {id} not found." )
	{ }
}
