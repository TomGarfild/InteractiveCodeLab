using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class QuickSortSimulationStrategy : IAlgorithmSimulationStrategy
{
    public List<VisualizationStep>? Simulate(Step[] steps, Dictionary<string, int[]> dataSet)
    {
        var visualizationSteps = new List<VisualizationStep>();
        var currentDataSet = (int[])dataSet["arr"].Clone();

        var variables = new Dictionary<string, int>();

        foreach (var step in steps)
        {
            if (step is VariableAssignmentStep vas)
            {
                if (vas.VariableName.Contains("[") && vas.VariableName.Contains("]"))
                {
                    var variableNameParts = vas.VariableName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    var arrayName = variableNameParts[0];
                    var indexExpression = variableNameParts[1];

                    var index = EvaluateExpression(indexExpression, variables);

                    currentDataSet[index] = int.Parse(vas.Value);
                }
                else
                {
                    variables[vas.VariableName] = int.Parse(vas.Value);
                }

                visualizationSteps.Add(new VisualizationStep
                {
                    DataState = (int[])currentDataSet.Clone(),
                    DelayMs = 0
                });
            }
            else if (step is FunctionStep { FunctionName: "quicksort" } fcs)
            {
                var start = EvaluateExpression(fcs.Arguments[0], variables);
                var end = EvaluateExpression(fcs.Arguments[1], variables);

                QuickSort(currentDataSet, start, end, visualizationSteps);
            }
        }

        return visualizationSteps;
    }

    private void QuickSort(int[] arr, int low, int high, List<VisualizationStep> visualizationSteps)
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high, visualizationSteps);

            QuickSort(arr, low, pi - 1, visualizationSteps);
            QuickSort(arr, pi + 1, high, visualizationSteps);
        }
    }

    private int Partition(int[] arr, int low, int high, List<VisualizationStep> visualizationSteps)
    {
        int pivot = arr[high];
        int i = (low - 1);

        for (int j = low; j <= high - 1; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                Swap(arr, i, j, visualizationSteps);
            }
        }
        Swap(arr, i + 1, high, visualizationSteps);
        return (i + 1);
    }

    private void Swap(int[] arr, int i, int j, List<VisualizationStep> visualizationSteps)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);

        visualizationSteps.Add(new VisualizationStep
        {
            DataState = (int[])arr.Clone(),
            DelayMs = 200
        });
    }

    private int EvaluateExpression(string expression, Dictionary<string, int> variables)
    {
        if (int.TryParse(expression, out int value))
        {
            return value;
        }

        if (variables.TryGetValue(expression, out var expression1))
        {
            return expression1;
        }

        throw new Exception($"Unknown variable or invalid expression: {expression}");
    }
}
