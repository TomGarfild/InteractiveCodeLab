using InteractiveCodeLab.Infrastructure.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace InteractiveCodeLab.Infrastructure.Models;

[MongoCollection("Algorithms")]
public record AlgorithmData(string Title, string Description, int[] Test1, int[] Test2, int[] Test3, int Version, DateTime LastUpdated) : IData
{
    public AlgorithmData() : this(string.Empty, string.Empty, Array.Empty<int>(), Array.Empty<int>(), Array.Empty<int>(), default, default) { }

    [BsonId] public string Id { get; init; } = null!;
}