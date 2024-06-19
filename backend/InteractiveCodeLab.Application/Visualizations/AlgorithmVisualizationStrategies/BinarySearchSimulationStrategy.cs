using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class BinarySearchSimulationStrategy : IAlgorithmSimulationStrategy
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
            else if (step is FunctionStep fcs && fcs.FunctionName == "binarysearch")
            {
                var target = EvaluateExpression(fcs.Arguments[0], variables);
                var left = 0;
                var right = currentDataSet.Length - 1;

                while (left <= right)
                {
                    int mid = left + (right - left) / 2;

                    visualizationSteps.Add(new VisualizationStep
                    {
                        DataState = (int[])currentDataSet.Clone(),
                        DelayMs = 200
                    });

                    if (currentDataSet[mid] == target)
                    {
                        break;
                    }
                    else if (currentDataSet[mid] < target)
                    {
                        left = mid + 1;
                    }
                    else
                    {
                        right = mid - 1;
                    }
                }
            }
        }

        return visualizationSteps;
    }

    private int EvaluateExpression(string expression, Dictionary<string, int> variables)
    {
        if (int.TryParse(expression, out int value))
        {
            return value;
        }
        else if (variables.ContainsKey(expression))
        {
            return variables[expression];
        }
        else
        {
            throw new Exception($"Unknown variable or invalid expression: {expression}");
        }
    }
}
