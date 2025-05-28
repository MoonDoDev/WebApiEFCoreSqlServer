using Application.Abstractions;
using Application.Employees;
using Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.ComponentModel.DataAnnotations;

namespace UnitTests;

internal class DbContextMock: DbContext, IAppDbContext
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
        optionsBuilder.UseInMemoryDatabase( "InMemoryDb" );
    }
}

public class CreateEmployee: IDisposable
{
    private readonly DbContextMock _dbContextMock = null!;
    private readonly EmployeeRepository _employeeRepositoryMock = null!;
    private readonly CreateCommandHandler _command = null!;

    public CreateEmployee()
    {
        _dbContextMock = new DbContextMock();
        _employeeRepositoryMock = new EmployeeRepository( _dbContextMock );
        _command = new CreateCommandHandler( _employeeRepositoryMock );
    }

    public void Dispose()
    {
        _dbContextMock?.Dispose();
        GC.SuppressFinalize( this );
    }

    private void Setup()
    {
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Alice Johnson", "Manager", 60000 ) );
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Bob Smith", "Accounter", 50000 ) );
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Charlie Brown", "Driver", 40000 ) );
        _dbContextMock.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateEmployee_AllFieldsFilledCorrectly_ReturnsEmployeeCreated()
    {
        // Arrange
        Setup();

        // Act
        var result = await _command.Execute( new CreateCommand
        {
            Name = "New Employee",
            Position = "Tester",
            Salary = 50000
        } );

        // Assert
        Assert.Equal( "New Employee", result.Name );
    }

    [Fact]
    public void CreateEmployee_EmptyName_ReturnsValidationError()
    {
        // Arrange
        Setup();

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( new CreateCommand
            {
                Name = string.Empty,
                Position = "Tester",
                Salary = 50000
            } ) );
    }

    [Fact]
    public void CreateEmployee_NameLengthLessThan3_ReturnsValidationError()
    {
        // Arrange
        Setup();

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( new CreateCommand
            {
                Name = "AB",
                Position = "Tester",
                Salary = 50000
            } ) );
    }

    [Fact]
    public void CreateEmployee_EmptyPosition_ReturnsValidationError()
    {
        // Arrange
        Setup();

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( new CreateCommand
            {
                Name = "New Employee",
                Position = string.Empty,
                Salary = 50000
            } ) );
    }

    [Fact]
    public void CreateEmployee_PositionLengthLessThan3_ReturnsValidationError()
    {
        // Arrange
        Setup();

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( new CreateCommand
            {
                Name = "New Employee",
                Position = "AB",
                Salary = 50000
            } ) );
    }

    [Fact]
    public void CreateEmployee_SalaryLessThan0_ReturnsValidationError()
    {
        // Arrange
        Setup();

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( new CreateCommand
            {
                Name = "New Employee",
                Position = "Tester",
                Salary = -50000
            } ) );
    }
}

public class UpdateEmployee: IDisposable
{
    private readonly DbContextMock _dbContextMock = null!;
    private readonly EmployeeRepository _employeeRepositoryMock = null!;
    private readonly UpdateCommandHandler _command = null!;

    public UpdateEmployee()
    {
        _dbContextMock = new DbContextMock();
        _employeeRepositoryMock = new EmployeeRepository( _dbContextMock );
        _command = new UpdateCommandHandler( _employeeRepositoryMock );
    }

    public void Dispose()
    {
        _dbContextMock?.Dispose();
        GC.SuppressFinalize( this );
    }

    private void Setup()
    {
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Alice Johnson", "Manager", 60000 ) );
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Bob Smith", "Accounter", 50000 ) );
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Charlie Brown", "Driver", 40000 ) );
        _dbContextMock.SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateEmployee_AllFieldsFilledCorrectly_ReturnsEmployeeUpdated()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var updEmployee = new UpdateCommand
        {
            Id = employee!.Id,
            Name = "Updated Name",
            Position = "Updated Position",
            Salary = 65000
        };

        // Act
        var updateResult = await _command.Execute( updEmployee );

        // Assert
        Assert.Equal( "Updated Name", updateResult.Name );
    }

    [Fact]
    public void UpdateEmployee_EmployeeIdNotFound_ReturnsNotFoundException()
    {
        // Arrange
        Setup();

        var updEmployee = new UpdateCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Name",
            Position = "Updated Position",
            Salary = 65000
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<NotFoundEmployeeException>(
            () => _command.Execute( updEmployee ) );
    }

    [Fact]
    public void UpdateEmployee_EmptyName_ReturnsValidationError()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var updEmployee = new UpdateCommand
        {
            Id = employee!.Id,
            Name = string.Empty,
            Position = "Updated Position",
            Salary = 65000
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( updEmployee ) );
    }

    [Fact]
    public void UpdateEmployee_NameLengthLessThan3_ReturnsValidationError()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var updEmployee = new UpdateCommand
        {
            Id = employee!.Id,
            Name = "AB",
            Position = "Updated Position",
            Salary = 65000
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( updEmployee ) );
    }

    [Fact]
    public void UpdateEmployee_EmptyPosition_ReturnsValidationError()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var updEmployee = new UpdateCommand
        {
            Id = employee!.Id,
            Name = "Updated Name",
            Position = string.Empty,
            Salary = 65000
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( updEmployee ) );
    }

    [Fact]
    public void UpdateEmployee_PositionLengthLessThan3_ReturnsValidationError()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var updEmployee = new UpdateCommand
        {
            Id = employee!.Id,
            Name = "Updated Name",
            Position = "AB",
            Salary = 65000
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( updEmployee ) );
    }

    [Fact]
    public void UpdateEmployee_SalaryLessThan0_ReturnsValidationError()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var updEmployee = new UpdateCommand
        {
            Id = employee!.Id,
            Name = "Updated Name",
            Position = "Updated Position",
            Salary = -65000
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( updEmployee ) );
    }
}

public class DeleteEmployee: IDisposable
{
    private readonly DbContextMock _dbContextMock = null!;
    private readonly EmployeeRepository _employeeRepositoryMock = null!;
    private readonly DeleteCommandHandler _command = null!;

    public DeleteEmployee()
    {
        _dbContextMock = new DbContextMock();
        _employeeRepositoryMock = new EmployeeRepository( _dbContextMock );
        _command = new DeleteCommandHandler( _employeeRepositoryMock );
    }

    public void Dispose()
    {
        _dbContextMock?.Dispose();
        GC.SuppressFinalize( this );
    }

    private void Setup()
    {
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Alice Johnson", "Manager", 60000 ) );
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Bob Smith", "Accounter", 50000 ) );
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Charlie Brown", "Driver", 40000 ) );
        _dbContextMock.SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteComployee_DataIsCorrect_ReturnsEmployeeDeleted()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var deleteEmployee = new DeleteCommand
        {
            Id = employee!.Id
        };

        // Act
        await _command.Execute( deleteEmployee );

        // Assert
        var deletedEmployee = await _employeeRepositoryMock.GetByIdAsync( employee.Id, CancellationToken.None );
        Assert.Null( deletedEmployee );
    }

    [Fact]
    public void DeleteComployee_EmployeeIdNotValid_ReturnsValidationError()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var deleteEmployee = new DeleteCommand
        {
            Id = Guid.Empty
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( deleteEmployee ) );
    }

    [Fact]
    public void DeleteComployee_EmployeeIdNotFound_ReturnsNotFoundException()
    {
        // Arrange
        Setup();

        var deleteEmployee = new DeleteCommand
        {
            Id = Guid.NewGuid()
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<NotFoundEmployeeException>(
            () => _command.Execute( deleteEmployee ) );
    }
}

public class GetEmployee: IDisposable
{
    private readonly DbContextMock _dbContextMock = null!;
    private readonly EmployeeRepository _employeeRepositoryMock = null!;
    private readonly GetByIdQueryHandler _command = null!;

    public GetEmployee()
    {
        _dbContextMock = new DbContextMock();
        _employeeRepositoryMock = new EmployeeRepository( _dbContextMock );
        _command = new GetByIdQueryHandler( _employeeRepositoryMock );
    }

    public void Dispose()
    {
        _dbContextMock?.Dispose();
        GC.SuppressFinalize( this );
    }

    private void Setup()
    {
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Alice Johnson", "Manager", 60000 ) );
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Bob Smith", "Accounter", 50000 ) );
        _dbContextMock.Employees.Add( new Employee( Guid.NewGuid(), "Charlie Brown", "Driver", 40000 ) );
        _dbContextMock.SaveChangesAsync();
    }

    [Fact]
    public async Task GetEmployee_EmployeeIdFound_ReturnsEmployee()
    {
        // Arrange
        Setup();

        var employees = _dbContextMock.Employees.ToList();
        var employee = employees.FirstOrDefault();

        var getEmployee = new GetByIdQuery
        {
            Id = employee!.Id
        };

        // Act
        var result = await _command.Execute( getEmployee );

        // Assert
        Assert.Equal( employee.Id, result.Id );
        Assert.Equal( employee.Name, result.Name );
        Assert.Equal( employee.Position, result.Position );
        Assert.Equal( employee.Salary, result.Salary );
    }

    [Fact]
    public void GetEmployee_EmployeeIdNotFound_ReturnsNotFoundException()
    {
        // Arrange
        Setup();

        var getEmployee = new GetByIdQuery
        {
            Id = Guid.NewGuid()
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<NotFoundEmployeeException>(
            () => _command.Execute( getEmployee ) );
    }

    [Fact]
    public void GetEmployee_EmployeeIdNotValid_ReturnsValidationError()
    {
        // Arrange
        Setup();

        var getEmployee = new GetByIdQuery
        {
            Id = Guid.Empty
        };

        // Act & Assert
        var result = Assert.ThrowsAsync<ValidationException>(
            () => _command.Execute( getEmployee ) );
    }
}
