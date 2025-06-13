using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class SecureStorage
{
    private static readonly string FilePath = "credentials.dat";

    public static void SaveCredentials(string email, string password)
    {
        var data = $"{email}|{password}";
        var encrypted = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(data),
            null,
            DataProtectionScope.CurrentUser);
        File.WriteAllBytes(FilePath, encrypted);
    }

    public static (string email, string password)? LoadCredentials()
    {
        if (!File.Exists(FilePath))
            return null;

        try
        {
            var encrypted = File.ReadAllBytes(FilePath);
            var decrypted = ProtectedData.Unprotect(
                encrypted,
                null,
                DataProtectionScope.CurrentUser);
            var parts = Encoding.UTF8.GetString(decrypted).Split('|');
            return (parts[0], parts[1]);
        }
        catch
        {
            return null;
        }
    }

    public static void ClearCredentials()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
