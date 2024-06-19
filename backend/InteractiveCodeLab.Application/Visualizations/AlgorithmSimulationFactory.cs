using InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InteractiveCodeLab.Application.Visualizations;

public class AlgorithmSimulationFactory : IAlgorithmSimulationFactory
{
    public IAlgorithmSimulationStrategy GetStrategy(string? algorithmId)
    {
        switch (algorithmId)
        {
            case "bubble-sort":
                return new BubbleSortSimulationStrategy();
            case "insertion-sort":
                return new BubbleSortSimulationStrategy();
            case "quick-sort":
                return new BubbleSortSimulationStrategy();
            case "merge-sort":
                return new MergeSortSimulationStrategy();
            case "selection-sort":
                return new SelectionSortSimulationStrategy();
            case "binary-search":
                return new BinarySearchSimulationStrategy();
            case "kmp":
                return new KMPAlgorithmSimulationStrategy();
            case "dijkstra":
                return new KMPAlgorithmSimulationStrategy();
            case "bfs":
                return new BFSAlgorithmSimulationStrategy();
            case "dfs":
                return new DFSAlgorithmSimulationStrategy();
            default:
                throw new NotImplementedException();
        }
    }
}

public interface IAlgorithmSimulationFactory
{
    public IAlgorithmSimulationStrategy GetStrategy(string? algorithmId);
}