using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;

namespace WebApi;

internal static class Extensions
{
    public static void AddWebApiServices( this IServiceCollection services )
    {
        services.AddApiVersioning( verOptions =>
        {
            verOptions.DefaultApiVersion = new ApiVersion( ApiVersions.Employees.CurrentMajor, 0 );
            verOptions.AssumeDefaultVersionWhenUnspecified = true;
            verOptions.ReportApiVersions = true;

            verOptions.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader( "X-Api-Version" ) );

        } ).AddApiExplorer( explorerOptions =>
        {
            explorerOptions.GroupNameFormat = "'v'V";
            explorerOptions.SubstituteApiVersionInUrl = true;
        } );
    }

    public static WebApplication MapWebApiServices( this WebApplication app )
    {
        app.MapHealthChecks( ApiEndpoints.Health.Live, new HealthCheckOptions
        {
            Predicate = ( _ ) => false
        } );

        app.MapHealthChecks( ApiEndpoints.Health.Ready, new HealthCheckOptions
        {
            Predicate = ( check ) => check.Tags.Contains( "ready" ),
            ResponseWriter = async ( context, report ) =>
            {
                var result = JsonSerializer.Serialize( new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select( entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                        duration = entry.Value.Duration.ToString(),
                    } )
                } );

                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync( result ).ConfigureAwait( false );
            }
        } );

        return app;
    }
}
