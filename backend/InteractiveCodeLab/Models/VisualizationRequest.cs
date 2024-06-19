using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Models;

public record VisualizationRequest
{
    public string SelectedLanguage { get; set; } = null!;
    public VisualizationRegime Regime { get; set; }
    public int[]? CustomData { get; set; }
}