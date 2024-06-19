using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class BFSAlgorithmSimulationStrategy : IAlgorithmSimulationStrategy
{
    public List<VisualizationStep>? Simulate(Step[] steps, Dictionary<string, int[]> dataSet)
    {
        var visualizationSteps = new List<VisualizationStep>();
        var graph = new Dictionary<int, List<int>>();
        var startNode = 0;
        var visited = new HashSet<int>();
        var queue = new Queue<int>();
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            int currentNode = queue.Dequeue();
            if (!visited.Contains(currentNode))
            {
                visited.Add(currentNode);
                AddVisualizationStep(visualizationSteps, visited, currentNode);

                foreach (var neighbor in graph[currentNode])
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
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
