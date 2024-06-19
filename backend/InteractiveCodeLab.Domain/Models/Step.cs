namespace InteractiveCodeLab.Domain.Models;

public abstract record Step(int Id)
{
}

public record VariableAssignmentStep(int Id) : Step(Id)
{
    public string VariableName { get; init; }
    public string Value { get; set; }
}

public record LoopStep(int Id) : Step(Id)
{
    public string LoopVariable  { get; init; }
    public string Iterable { get; init; }
    public List<int> Steps { get; set; } = new List<int>();

    public override string ToString()
    {
        return $"For: {LoopVariable} in {Iterable}, Steps: [{string.Join(", ", Steps)}]";
    }
}

public record ConditionStep(int Id) : Step(Id)
{
    public string? Condition { get; init; }
    public List<int> Steps { get; set; } = new List<int>();

    public override string ToString()
    {
        return $"Condition: {Condition}, Steps: [{string.Join(", ", Steps)}]";
    }
}

public record FunctionStep(int Id, string FunctionName) : Step(Id)
{
    public List<string> Arguments { get; set; } = new List<string>();
    public List<int> Steps { get; set; } = new List<int>();

    public override string ToString()
    {
        return $"Function: {FunctionName}, Arguments: [{string.Join(", ", Arguments)}], Steps: [{string.Join(", ", Steps)}]";
    }
}

public record CallStep(int Id, string FunctionName) : Step(Id)
{
    public List<int> Arguments { get; set; } = new List<int>();

    public override string ToString()
    {
        return $"Call: {FunctionName}, Arguments: [{string.Join(", ", Arguments)}]";
    }
}