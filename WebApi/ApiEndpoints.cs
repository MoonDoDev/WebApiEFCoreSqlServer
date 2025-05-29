namespace WebApi;

internal static class ApiEndpoints
{
    private const string API_BASE = "api/v{v:apiVersion}";

    internal static class Employees
    {
        private const string BASE = $"{API_BASE}/employees";

        public const string Create = BASE;
        public const string GetAll = BASE;
        public const string GetOne = $"{BASE}/{{id:guid}}";
        public const string Update = BASE;
        public const string Delete = $"{BASE}/{{id:guid}}";
    }

    internal static class Health
    {
        private const string BASE = $"{API_BASE}/health";

        public const string Live = $"{BASE}/live";
        public const string Ready = $"{BASE}/ready";
    }
}