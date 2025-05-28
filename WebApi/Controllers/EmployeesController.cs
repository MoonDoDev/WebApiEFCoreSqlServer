using Application.Employees;
using Asp.Versioning;
using Domain.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Persistence;

namespace WebApi.Controllers;

[ApiController]
[ApiVersion( Commons.EMPLOYEE_API_VER_1 )]
[Route( Commons.EMPLOYEE_API_ROUTE )]
public class EmployeesController( ILogger<EmployeesController> logger ): ControllerBase
{
    private readonly ILogger<EmployeesController> _logger = logger ??
        throw new ArgumentNullException( nameof( logger ) );

    [HttpGet( "all" )]
    [MapToApiVersion( Commons.EMPLOYEE_API_VER_1 )]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(
        IQueryAllHandler queryHandler, IMemoryCache localCache )
    {
        try
        {
            if ( !localCache.TryGetValue( Commons.EMPLOYEE_CACHE_KEY, out IEnumerable<Employee>? employees ) )
            {
                employees = await queryHandler.Execute();

                localCache.Set(
                    Commons.EMPLOYEE_CACHE_KEY,
                    employees,
                    TimeSpan.FromMinutes( 5 ) );
            }

            FastLogger.LogInfo( _logger, $"Fetched {employees!.Count()} employees.", null );
            return Ok( employees );
        }
        catch ( Exception e )
        {
            FastLogger.LogError( _logger, "GetEmployees throws an exception", e );
            return BadRequest( e.Message );
        }
    }

    [HttpGet( "one" )]
    [MapToApiVersion( Commons.EMPLOYEE_API_VER_1 )]
    public async Task<ActionResult<Employee>> GetEmployee(
        [FromQuery] Guid id,
        IQueryOneHandler queryHandler,
        IMemoryCache? localCache = null )
    {
        try
        {
            localCache ??= HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

            if ( !localCache.TryGetValue( id, out Employee? employee ) )
            {
                var employeeId = new GetByIdQuery { Id = id };
                employee = await queryHandler.Execute( employeeId );
                localCache.Set( id, employee, TimeSpan.FromMinutes( 5 ) );
            }

            FastLogger.LogInfo( _logger, $"Fetched employee with id {id}", null );
            return Ok( employee );
        }
        catch ( Exception e )
        {
            FastLogger.LogError( _logger, "GetEmployee throws an exception", e );
            return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
        }
    }

    [HttpPost( "new" )]
    [MapToApiVersion( Commons.EMPLOYEE_API_VER_1 )]
    public async Task<ActionResult<Employee>> CreateEmployee(
        [FromBody] CreateCommand employee,
        ICreateCommandHandler commandHandler,
        IMemoryCache? localCache = null )
    {
        try
        {
            var result = await commandHandler.Execute( employee );
            FastLogger.LogInfo( _logger, $"Created employee with id {result.Id}", null );

            localCache ??= HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            localCache.Remove( Commons.EMPLOYEE_CACHE_KEY );

            return CreatedAtAction( nameof( GetEmployee ), new { id = result.Id }, result );
        }
        catch ( Exception e )
        {
            FastLogger.LogError( _logger, "CreateEmployee throws an exception", e );
            return BadRequest( e.Message );
        }
    }

    [HttpPut( "upd" )]
    [MapToApiVersion( Commons.EMPLOYEE_API_VER_1 )]
    public async Task<ActionResult<Employee>> UpdateEmployee(
        [FromBody] UpdateCommand employee,
        IUpdateCommandHandler commandHandler,
        IMemoryCache? localCache = null )
    {
        try
        {
            var result = await commandHandler.Execute( employee );
            FastLogger.LogInfo( _logger, $"Updated employee with id {result.Id}", null );

            localCache ??= HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            localCache.Remove( Commons.EMPLOYEE_CACHE_KEY );
            localCache.Remove( result.Id );

            return Ok( result );
        }
        catch ( Exception e )
        {
            FastLogger.LogError( _logger, "UpdateEmployee throws an exception", e );
            return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
        }
    }

    [HttpDelete( "del" )]
    [MapToApiVersion( Commons.EMPLOYEE_API_VER_1 )]
    public async Task<ActionResult> DeleteEmployee(
        [FromQuery] Guid id,
        IDeleteCommandHandler commandHandler,
        IMemoryCache? localCache = null )
    {
        try
        {
            var employeeId = new DeleteCommand { Id = id };
            await commandHandler.Execute( employeeId );

            FastLogger.LogInfo( _logger, $"Deleted employee with id {id}", null );

            localCache ??= HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            localCache.Remove( Commons.EMPLOYEE_CACHE_KEY );
            localCache.Remove( employeeId.Id );

            return NoContent();
        }
        catch ( Exception e )
        {
            FastLogger.LogError( _logger, "DeleteEmployee throws an exception", e );
            return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
        }
    }
}
