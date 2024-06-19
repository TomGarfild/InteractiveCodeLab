using ErrorOr;

namespace InteractiveCodeLab.Infrastructure;

public sealed class PasswordHasher : IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}