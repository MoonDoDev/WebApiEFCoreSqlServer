namespace Persistence;

public static class Commons
{
    public const string SETTINGS_FILE_NAME = "appsettings.json";
    public const string DB_CONNECTION_NAME = "DefaultConnection";

    public const string EMPLOYEE_API_ROUTE = "api/v{v:apiVersion}/employees";
    public const string API_LIVES_ENDPOINT = "api/v{v:apiVersion}/health/live";
    public const string API_READY_ENDPOINT = "api/v{v:apiVersion}/health/ready";
    public const string EMPLOYEE_CACHE_KEY = "all-employees";

    public const double EMPLOYEE_API_VER_1 = 1.0;
    public const int EMPLOYEE_MAIN_API_VER = 1;
}
