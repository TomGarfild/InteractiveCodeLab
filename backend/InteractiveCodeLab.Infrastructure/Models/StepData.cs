namespace InteractiveCodeLab.Infrastructure.Models;


public abstract record StepData(int Id, int Version, DateTime LastUpdated) : IData
{
    string IData.Id => Id.ToString();

    public int Version { get; } = Version;
    public DateTime LastUpdated { get; } = LastUpdated;
}

public record VariableAssignmentStepData(int Id, int Version, DateTime LastUpdated) : StepData(Id, Version, LastUpdated)
{
    public VariableAssignmentStepData() : this(default, default, default)
    {
        
    }
    public string VariableName { get; init; }
    public string Value { get; set; }
}

public record LoopStepData(int Id, int Version, DateTime LastUpdated) : StepData(Id, Version, LastUpdated)
{
    public LoopStepData() : this(default, default, default)
    {
        
    }
    public string LoopVariable  { get; init; }
    public string Iterable { get; init; }
    public int[] Steps { get; set; }
}

public record ConditionStepData(int Id, int Version, DateTime LastUpdated) : StepData(Id, Version, LastUpdated)
{
    public ConditionStepData() : this(default, default, default)
    {
        
    }
    public string? Condition { get; init; }
    public int[] Steps { get; set; }
}

public record FunctionStepData(int Id, int Version, DateTime LastUpdated, string FunctionName) : StepData(Id, Version, LastUpdated)
{
    public FunctionStepData() : this(default, default, default, default)
    {
        
    }
    public string[] Arguments { get; set; }
    public int[] Steps { get; set; }
}

public record CallStepData(int Id, int Version, DateTime LastUpdated, string FunctionName) : StepData(Id, Version, LastUpdated)
{
    public CallStepData() : this(default, default, default, default)
    {
        
    }
    public int[] Arguments { get; set; }
}