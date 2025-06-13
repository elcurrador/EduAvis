using EduAvis.Backend.Model;

public static class SessionManager
{
    public static Teacher CurrentUser { get; set; }
    public static List<string> Roles { get; set; } = new();

    public static bool HasPermission(string code) => Roles.Contains(code);
}
