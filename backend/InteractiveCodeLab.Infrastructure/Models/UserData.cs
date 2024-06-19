using InteractiveCodeLab.Infrastructure.Attributes;

namespace InteractiveCodeLab.Infrastructure.Models;

[MongoCollection("Users")]
public class UserData : IData
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PasswordHash { get; init; }
    public int Version { get; }
    public DateTime LastUpdated { get; }
}