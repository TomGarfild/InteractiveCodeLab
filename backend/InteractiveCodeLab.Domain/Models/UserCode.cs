namespace InteractiveCodeLab.Domain.Models;

public record UserCode(string Id, UserCodeKey Key, string Code)
{
    public static UserCode Default = new UserCode("", new UserCodeKey("", "", ""), "");
}