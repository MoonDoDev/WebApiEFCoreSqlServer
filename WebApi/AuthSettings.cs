namespace WebApi;

internal static class AuthSettings
{
    public static class PolicyNames
    {
        public const string AdminUser = "Admin";
        public const string TrustedMember = "Trusted";
    }

    public static class ClaimNames
    {
        public const string AdminUser = "admin";
        public const string TrustedMember = "trusted_member";
    }
}
