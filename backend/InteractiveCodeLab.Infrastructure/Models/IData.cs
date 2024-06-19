namespace InteractiveCodeLab.Infrastructure.Models;

public interface IData
{
    public string Id { get; }
    public int Version { get; }
    public DateTime LastUpdated { get; }
}