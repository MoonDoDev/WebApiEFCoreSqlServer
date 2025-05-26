namespace Domain.Employees;

public class NotFoundEmployeeException( Guid id )
    : Exception( $"Employee with ID {id} not found." )
{ }
