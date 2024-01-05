using DevOne.Security.Cryptography.BCrypt;

namespace EzTech.Api.Authentication;

public static class PasswordHelper
{
    public static string HashPassword(string message, string salt)
    {
        var hashedPassword = BCryptHelper.HashPassword(message, salt);
        return hashedPassword;
    }
    public static string GenerateSalt()
    {
        const int workFactor = 5;
        var salt = BCryptHelper.GenerateSalt(workFactor);
        return salt;
    }
}