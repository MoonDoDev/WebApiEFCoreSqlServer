using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Persistence;
using System.Net.Mime;
using System.Text.Json;

namespace WebApi;

internal static class Extensions
{
    public static void AddWebApiServices( this IServiceCollection services )
    {
        services.AddApiVersioning( verOptions =>
        {
            verOptions.DefaultApiVersion = new ApiVersion( Commons.EMPLOYEE_MAIN_API_VER );
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
        app.MapHealthChecks( Commons.API_LIVES_ENDPOINT, new HealthCheckOptions
        {
            Predicate = ( _ ) => false
        } );

        app.MapHealthChecks( Commons.API_READY_ENDPOINT, new HealthCheckOptions
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
