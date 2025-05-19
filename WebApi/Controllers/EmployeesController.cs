using Application.Employees;
using Domain.Employees;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route( "[controller]" )]
public class EmployeesController( ILogger<EmployeesController> logger ) : ControllerBase
{
	private readonly ILogger<EmployeesController> _logger = logger ??
		throw new ArgumentNullException( nameof( logger ) );

	[HttpGet( "/getall" )]
	public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(
		GetAllQueryHandler queryHandler )
	{
		try
		{
			var employees = await queryHandler.Handle();

			_logger.LogInformation( "Fetched {Count} employees", employees.Count() );
			return Ok( employees );
		}
		catch ( Exception e )
		{
			_logger.LogError( "GetEmployees throws an exception: {Msg}", e.Message );
			return BadRequest( e.Message );
		}
	}

	[HttpGet( "/getone/{id:guid}" )]
	public async Task<ActionResult<Employee>> GetEmployee(
		Guid id,
		GetByIdQueryHandler queryHandler )
	{
		try
		{
			var employeeId = new GetByIdQuery { Id = id };
			var employee = await queryHandler.Handle( employeeId );

			_logger.LogInformation( "Fetched employee with id {Id}", id );
			return Ok( employee );
		}
		catch ( Exception e )
		{
			_logger.LogError( "GetEmployee throws an exception: {Msg}", e.Message );
			return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
		}
	}

	[HttpPost( "/create" )]
	public async Task<ActionResult<Employee>> CreateEmployee(
		CreateCommand employee,
		CreateCommandHandler commandHandler )
	{
		try
		{
			var result = await commandHandler.Handle( employee );

			_logger.LogInformation( "Created employee with id {Id}", result.Id );
			return CreatedAtAction( nameof( GetEmployee ), new { id = result.Id }, result );
		}
		catch ( Exception e )
		{
			_logger.LogError( "CreateEmployee throws an exception: {Msg}", e.Message );
			return BadRequest( e.Message );
		}
	}

	[HttpPut( "/update" )]
	public async Task<ActionResult<Employee>> UpdateEmployee(
		UpdateCommand employee,
		UpdateCommandHandler commandHandler )
	{
		try
		{
			var result = await commandHandler.Handle( employee );

			_logger.LogInformation( "Updated employee with id {Id}", result.Id );
			return Ok( result );
		}
		catch ( Exception e )
		{
			_logger.LogError( "UpdateEmployee throws an exception: {Msg}", e.Message );
			return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
		}
	}

	[HttpDelete( "/delete/{id:guid}" )]
	public async Task<ActionResult> DeleteEmployee(
		Guid id,
		DeleteCommandHandler commandHandler )
	{
		try
		{
			var employeeId = new DeleteCommand { Id = id };
			await commandHandler.Handle( employeeId );

			_logger.LogInformation( "Deleted employee with id {Id}", id );
			return NoContent();
		}
		catch ( Exception e )
		{
			_logger.LogError( "DeleteEmployee throws an exception: {Msg}", e.Message );
			return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
		}
	}
}
