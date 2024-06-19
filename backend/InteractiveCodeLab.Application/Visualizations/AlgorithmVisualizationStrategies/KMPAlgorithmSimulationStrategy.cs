using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class KMPAlgorithmSimulationStrategy : IAlgorithmSimulationStrategy
{
    public List<VisualizationStep>? Simulate(Step[] steps, Dictionary<string, int[]> dataSet)
    {
        var visualizationSteps = new List<VisualizationStep>();
        var text = "";
        var pattern = "";
        var currentText = text.ToCharArray();
        var currentPattern = pattern.ToCharArray();

        var lps = ComputeLPSArray(pattern);

        int i = 0;
        int j = 0;
        while (i < text.Length)
        {
            if (pattern[j] == text[i])
            {
                j++;
                i++;
            }

            if (j == pattern.Length)
            {
                // Pattern found at index i - j
                AddVisualizationStep(visualizationSteps, currentText, currentPattern, i - j, j);
                j = lps[j - 1];
            }
            else if (i < text.Length && pattern[j] != text[i])
            {
                if (j != 0)
                {
                    j = lps[j - 1];
                }
                else
                {
                    i++;
                }
            }
            AddVisualizationStep(visualizationSteps, currentText, currentPattern, i, j);
        }

        return visualizationSteps;
    }

    private void AddVisualizationStep(List<VisualizationStep> visualizationSteps, char[] text, char[] pattern, int textIndex, int patternIndex)
    {
        visualizationSteps.Add(new VisualizationStep
        {
            DelayMs = 200
        });
    }

    private int[] ComputeLPSArray(string pattern)
    {
        int length = 0;
        int i = 1;
        int[] lps = new int[pattern.Length];
        lps[0] = 0;

        while (i < pattern.Length)
        {
            if (pattern[i] == pattern[length])
            {
                length++;
                lps[i] = length;
                i++;
            }
            else
            {
                if (length != 0)
                {
                    length = lps[length - 1];
                }
                else
                {
                    lps[i] = 0;
                    i++;
                }
            }
        }
        return lps;
    }
}
