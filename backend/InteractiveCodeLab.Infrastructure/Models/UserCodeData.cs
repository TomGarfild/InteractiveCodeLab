using InteractiveCodeLab.Infrastructure.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace InteractiveCodeLab.Infrastructure.Models;

[MongoCollection("UserCode")]
public record UserCodeData : IData
{
    [BsonId] public string Id { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public string AlgorithmId { get; init; } = null!;
    public string SelectedLanguage { get; init; } = null!;
    public string Code { get; init; } = null!;
    public int Version { get; init; }
    public DateTime LastUpdated { get; init; }
}