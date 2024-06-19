using InteractiveCodeLab.Infrastructure.Models;

namespace InteractiveCodeLab.Infrastructure;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserData user);
}