namespace InteractiveCodeLab.Domain.Models;

public record VisualizationStep
{
    public int[] DataState { get; set; }
    public int DelayMs { get; set; }
}