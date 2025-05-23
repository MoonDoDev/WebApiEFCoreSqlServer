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
		IQueryAllHandler queryHandler )
	{
		try
		{
			var employees = await queryHandler.Execute();

			FastLogger.LogInfo( _logger, $"Fetched {employees.Count()} employees", null );
			return Ok( employees );
		}
		catch ( Exception e )
		{
			FastLogger.LogError( _logger, "GetEmployees throws an exception", e );
			return BadRequest( e.Message );
		}
	}

	[HttpGet( "/getone/{id:guid}" )]
	public async Task<ActionResult<Employee>> GetEmployee(
		Guid id,
		IQueryOneHandler queryHandler )
	{
		try
		{
			var employeeId = new GetByIdQuery { Id = id };
			var employee = await queryHandler.Execute( employeeId );

			FastLogger.LogInfo( _logger, $"Fetched employee with id {id}", null );
			return Ok( employee );
		}
		catch ( Exception e )
		{
			FastLogger.LogError( _logger, "GetEmployee throws an exception", e );
			return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
		}
	}

	[HttpPost( "/create" )]
	public async Task<ActionResult<Employee>> CreateEmployee(
		CreateCommand employee,
		ICreateCommandHandler commandHandler )
	{
		try
		{
			var result = await commandHandler.Execute( employee );

			FastLogger.LogInfo( _logger, $"Created employee with id {result.Id}", null );
			return CreatedAtAction( nameof( GetEmployee ), new { id = result.Id }, result );
		}
		catch ( Exception e )
		{
			FastLogger.LogError( _logger, "CreateEmployee throws an exception", e );
			return BadRequest( e.Message );
		}
	}

	[HttpPut( "/update" )]
	public async Task<ActionResult<Employee>> UpdateEmployee(
		UpdateCommand employee,
		IUpdateCommandHandler commandHandler )
	{
		try
		{
			var result = await commandHandler.Execute( employee );

			FastLogger.LogInfo( _logger, $"Updated employee with id {result.Id}", null );
			return Ok( result );
		}
		catch ( Exception e )
		{
			FastLogger.LogError( _logger, "UpdateEmployee throws an exception", e );
			return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
		}
	}

	[HttpDelete( "/delete/{id:guid}" )]
	public async Task<ActionResult> DeleteEmployee(
		Guid id,
		IDeleteCommandHandler commandHandler )
	{
		try
		{
			var employeeId = new DeleteCommand { Id = id };
			await commandHandler.Execute( employeeId );

			FastLogger.LogInfo( _logger, $"Deleted employee with id {id}", null );
			return NoContent();
		}
		catch ( Exception e )
		{
			FastLogger.LogError( _logger, "DeleteEmployee throws an exception", e );
			return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
		}
	}
}
