using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Application;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace WebApi;

internal static class Extensions
{
    public static IServiceCollection AddWebApiServices(
        this IServiceCollection services, IConfiguration configuration )
    {
        services.AddAuthentication( x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        } ).AddJwtBearer( x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(
                    ApiSettings.BuildJwtKey( configuration.GetValue<string>( ApiSettings.Key )! ) ) ),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = configuration.GetValue<string>( ApiSettings.Issuer )!,
                ValidAudience = configuration.GetValue<string>( ApiSettings.Audience )!,
                ValidateIssuer = true,
                ValidateAudience = true
            };
        } );

        services.AddAuthorizationBuilder()
            .AddPolicy( AuthSettings.PolicyNames.AdminUser, p => p.RequireClaim( AuthSettings.ClaimNames.AdminUser, "true" ) )
            .AddPolicy( AuthSettings.PolicyNames.TrustedMember, p => p.RequireAssertion( c =>
                c.User.HasClaim( m => m is { Type: AuthSettings.ClaimNames.AdminUser, Value: "true" } ) ||
                c.User.HasClaim( m => m is { Type: AuthSettings.ClaimNames.TrustedMember, Value: "true" } ) ) );

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

        return services;
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

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
