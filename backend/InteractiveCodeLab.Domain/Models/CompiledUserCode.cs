namespace InteractiveCodeLab.Domain.Models;

public record CompiledUserCode(string Id, UserCodeKey Key, Step[] Steps)
{
    public static CompiledUserCode Default = new("", new UserCodeKey("", "", ""), Array.Empty<Step>());
}