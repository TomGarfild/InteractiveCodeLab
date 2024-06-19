using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class DijkstraAlgorithmSimulationStrategy : IAlgorithmSimulationStrategy
{
    public List<VisualizationStep>? Simulate(Step[] steps, Dictionary<string, int[]> dataSet)
    {
        var visualizationSteps = new List<VisualizationStep>();
        var graph = new Dictionary<int, List<(int, int)>>();
        var startNode = 0;
        var numNodes = graph.Keys.Max() + 1;
        
        var distances = new int[numNodes];
        var previous = new int?[numNodes];
        var unvisited = new HashSet<int>(Enumerable.Range(0, numNodes));

        for (int i = 0; i < numNodes; i++)
        {
            distances[i] = int.MaxValue;
            previous[i] = null;
        }
        distances[startNode] = 0;

        while (unvisited.Count > 0)
        {
            int currentNode = unvisited.OrderBy(node => distances[node]).First();
            unvisited.Remove(currentNode);

            if (distances[currentNode] == int.MaxValue)
                break;

            foreach (var (neighbor, weight) in graph[currentNode])
            {
                if (unvisited.Contains(neighbor))
                {
                    int newDist = distances[currentNode] + weight;
                    if (newDist < distances[neighbor])
                    {
                        distances[neighbor] = newDist;
                        previous[neighbor] = currentNode;
                        AddVisualizationStep(visualizationSteps, distances, previous, currentNode, neighbor);
                    }
                }
            }
        }

        return visualizationSteps;
    }

    private void AddVisualizationStep(List<VisualizationStep> visualizationSteps, int[] distances, int?[] previous, int currentNode, int neighbor)
    {
        visualizationSteps.Add(new VisualizationStep
        {
            DelayMs = 200
        });
    }
}
