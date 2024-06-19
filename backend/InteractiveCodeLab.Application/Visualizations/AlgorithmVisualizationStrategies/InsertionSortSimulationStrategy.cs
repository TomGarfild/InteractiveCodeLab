using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class InsertionSortSimulationStrategy : IAlgorithmSimulationStrategy
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
            else if (step is LoopStep ls)
            {
                if (ls.Iterable.StartsWith("range"))
                {
                    var rangeParams = ls.Iterable.Substring(6, ls.Iterable.Length - 7).Split(',');
                    int start = int.Parse(rangeParams[0]);
                    int end = int.Parse(rangeParams[1]);

                    for (int i = start; i < end; i++)
                    {
                        variables[ls.LoopVariable] = i;

                        var key = currentDataSet[i];
                        int j = i - 1;

                        while (j >= 0 && currentDataSet[j] > key)
                        {
                            currentDataSet[j + 1] = currentDataSet[j];
                            j--;

                            visualizationSteps.Add(new VisualizationStep
                            {
                                DataState = (int[])currentDataSet.Clone(),
                                DelayMs = 200
                            });
                        }
                        currentDataSet[j + 1] = key;

                        visualizationSteps.Add(new VisualizationStep
                        {
                            DataState = (int[])currentDataSet.Clone(),
                            DelayMs = 200
                        });
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

        if (variables.TryGetValue(expression, value: out var expression1))
        {
            return expression1;
        }

        throw new Exception($"Unknown variable or invalid expression: {expression}");
    }
}