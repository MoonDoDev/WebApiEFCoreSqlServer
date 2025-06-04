namespace Application;

public static class ApiSettings
{
    public static string Key => $"{nameof( ApiSettings )}:Key";
    public static string Issuer => $"{nameof( ApiSettings )}:Issuer";
    public static string Audience => $"{nameof( ApiSettings )}:Audience";

    public static string BuildJwtKey( string? secret )
    {
        return $"Moon|{secret}|DoDev";
    }
}