using Application.Employees;
using Asp.Versioning;
using Domain.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Persistence;

namespace WebApi.Controllers;

[ApiController]
[ApiVersion( ApiVersions.Employees.Current, Deprecated = false )]
public class EmployeesController( ILogger<EmployeesController> logger ): ControllerBase
{
    private readonly ILogger<EmployeesController> _logger = logger ??
        throw new ArgumentNullException( nameof( logger ) );

    [HttpGet( ApiEndpoints.Employees.GetAll )]
    [MapToApiVersion( ApiVersions.Employees.Current )]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(
        IQueryAllHandler queryHandler,
        IMemoryCache localCache,
        CancellationToken cancellationToken = default )
    {
        try
        {
            if ( !localCache.TryGetValue( Constants.EmployeeCacheKey, out IEnumerable<Employee>? employees ) )
            {
                employees = await queryHandler.Execute( cancellationToken );
                localCache.Set( Constants.EmployeeCacheKey, employees, TimeSpan.FromMinutes( 5 ) );
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

    [HttpGet( ApiEndpoints.Employees.GetOne )]
    [MapToApiVersion( ApiVersions.Employees.Current )]
    public async Task<ActionResult<Employee>> GetEmployee(
        [FromRoute] Guid id,
        IQueryOneHandler queryHandler,
        IMemoryCache? localCache = null,
        CancellationToken cancellationToken = default )
    {
        try
        {
            localCache ??= HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

            if ( !localCache.TryGetValue( id, out Employee? employee ) )
            {
                var employeeId = new GetByIdQuery { Id = id };

                employee = await queryHandler.Execute( employeeId, cancellationToken );
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

    [HttpPost( ApiEndpoints.Employees.Create )]
    [MapToApiVersion( ApiVersions.Employees.Current )]
    public async Task<ActionResult<Employee>> CreateEmployee(
        [FromBody] CreateCommand employee,
        ICreateCommandHandler commandHandler,
        IMemoryCache? localCache = null,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var result = await commandHandler.Execute( employee, cancellationToken );
            FastLogger.LogInfo( _logger, $"Created employee with id {result.Id}", null );

            localCache ??= HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            localCache.Remove( Constants.EmployeeCacheKey );

            return CreatedAtAction( nameof( GetEmployee ), new { id = result.Id }, result );
        }
        catch ( Exception e )
        {
            FastLogger.LogError( _logger, "CreateEmployee throws an exception", e );
            return BadRequest( e.Message );
        }
    }

    [HttpPut( ApiEndpoints.Employees.Update )]
    [MapToApiVersion( ApiVersions.Employees.Current )]
    public async Task<ActionResult<Employee>> UpdateEmployee(
        [FromBody] UpdateCommand employee,
        IUpdateCommandHandler commandHandler,
        IMemoryCache? localCache = null,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var result = await commandHandler.Execute( employee, cancellationToken );
            FastLogger.LogInfo( _logger, $"Updated employee with id {result.Id}", null );

            localCache ??= HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            localCache.Remove( Constants.EmployeeCacheKey );
            localCache.Remove( result.Id );

            return Ok( result );
        }
        catch ( Exception e )
        {
            FastLogger.LogError( _logger, "UpdateEmployee throws an exception", e );
            return ( e is NotFoundEmployeeException ) ? NotFound( e.Message ) : BadRequest( e.Message );
        }
    }

    [HttpDelete( ApiEndpoints.Employees.Delete )]
    [MapToApiVersion( ApiVersions.Employees.Current )]
    public async Task<ActionResult> DeleteEmployee(
        [FromRoute] Guid id,
        IDeleteCommandHandler commandHandler,
        IMemoryCache? localCache = null,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var employeeId = new DeleteCommand { Id = id };
            await commandHandler.Execute( employeeId, cancellationToken );

            FastLogger.LogInfo( _logger, $"Deleted employee with id {id}", null );

            localCache ??= HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            localCache.Remove( Constants.EmployeeCacheKey );
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
