namespace Domain.Employees;

public interface IEmployeeRepository
{
	Task<Employee?> GetByIdAsync( Guid id, CancellationToken cancellationToken );
	Task AddAsync( Employee employee, CancellationToken cancellationToken );
	Task UpdateAsync( Employee employee, CancellationToken cancellationToken );
	Task DeleteAsync( Employee employee, CancellationToken cancellationToken );
}
