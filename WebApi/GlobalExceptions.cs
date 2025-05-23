using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

internal class GlobalExceptionsHandler(
	ILogger<GlobalExceptionsHandler> logger ) : IExceptionHandler
{
	private readonly ILogger<GlobalExceptionsHandler> _logger = logger;

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken )
	{
		FastLogger.LogError( _logger, string.Empty, exception );

		var problemDetails = new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "Internal Server Error",
			Instance = httpContext.Request.Path,
		};

		httpContext.Response.StatusCode = problemDetails.Status.Value;
		await httpContext.Response.WriteAsJsonAsync( problemDetails, cancellationToken );
		return true;
	}
}
