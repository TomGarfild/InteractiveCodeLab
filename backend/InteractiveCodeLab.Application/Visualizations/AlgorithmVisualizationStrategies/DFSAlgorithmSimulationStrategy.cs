using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class DFSAlgorithmSimulationStrategy : IAlgorithmSimulationStrategy
{
    public List<VisualizationStep>? Simulate(Step[] steps, Dictionary<string, int[]> dataSet)
    {
        var visualizationSteps = new List<VisualizationStep>();
        var graph = new Dictionary<int, List<int>>();
        var startNode = 0;
        var visited = new HashSet<int>();
        var stack = new Stack<int>();
        stack.Push(startNode);

        while (stack.Count > 0)
        {
            int currentNode = stack.Pop();
            if (!visited.Contains(currentNode))
            {
                visited.Add(currentNode);
                AddVisualizationStep(visualizationSteps, visited, currentNode);

                foreach (var neighbor in graph[currentNode])
                {
                    if (!visited.Contains(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                }
            }
        }

        return visualizationSteps;
    }

    private void AddVisualizationStep(List<VisualizationStep> visualizationSteps, HashSet<int> visited, int currentNode)
    {
        visualizationSteps.Add(new VisualizationStep
        {
            DelayMs = 200
        });
    }
}
