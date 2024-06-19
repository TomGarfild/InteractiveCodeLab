using ErrorOr;

namespace InteractiveCodeLab.Infrastructure;

public interface IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}