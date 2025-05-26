namespace WebApi;

using Asp.Versioning;
using WebApi.Controllers;

internal static class Extensions
{
    public static void AddWebApiServices( this IServiceCollection services )
    {
        services.AddApiVersioning( verOptions =>
        {
            verOptions.DefaultApiVersion = new ApiVersion(
                EmployeesController.EMPLOYEE_CUR_MAJOR_VER,
                EmployeesController.EMPLOYEE_CUR_MINOR_VER,
                null );

            verOptions.ReportApiVersions = true;
            verOptions.AssumeDefaultVersionWhenUnspecified = true;

            verOptions.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader( "X-Api-Version" ) );

        } ).AddApiExplorer( explorerOptions =>
        {
            explorerOptions.GroupNameFormat = "'v'V";
            explorerOptions.SubstituteApiVersionInUrl = true;
        } );


    }
}