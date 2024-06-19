using InteractiveCodeLab.Infrastructure.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace InteractiveCodeLab.Infrastructure.Models;

[MongoCollection("CompiledUserCode")]
public record CompiledUserCodeData : IData
{
    [BsonId] public string Id { get; init; } = null!;
    public string UserId { get; init; } = null!;
    public string AlgorithmId { get; init; } = null!;
    public string SelectedLanguage { get; init; } = null!;
    public bool CompiledSuccessfully { get; init; }
    public VariableAssignmentStepData[] VariableAssignmentSteps { get; init; } = null!;
    public LoopStepData[] LoopSteps { get; init; } = null!;
    public ConditionStepData[] ConditionSteps { get; init; } = null!;
    public int Version { get; init; }
    public DateTime LastUpdated { get; init; }
}