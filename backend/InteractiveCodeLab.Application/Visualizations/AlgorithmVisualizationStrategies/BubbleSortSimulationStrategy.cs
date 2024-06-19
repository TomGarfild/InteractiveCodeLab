using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class BubbleSortSimulationStrategy : IAlgorithmSimulationStrategy
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
                    var variableNameParts =
                        vas.VariableName.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
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

                        visualizationSteps.Add(new VisualizationStep
                        {
                            DataState = (int[])currentDataSet.Clone(),
                            DelayMs = 200
                        });

                        if (i < currentDataSet.Length - 1)
                        {
                            if (currentDataSet[i] > currentDataSet[i + 1])
                            {
                                // Swap elements
                                (currentDataSet[i], currentDataSet[i + 1]) = (currentDataSet[i + 1], currentDataSet[i]);

                                visualizationSteps.Add(new VisualizationStep
                                {
                                    DataState = (int[])currentDataSet.Clone(),
                                    DelayMs = 200
                                });
                            }
                        }
                    }
                }
                else if (ls.Steps.Any())
                {
                    // Iterate over the predefined list of steps
                    foreach (var stepValue in ls.Steps)
                    {
                        variables[ls.LoopVariable] = stepValue;

                        visualizationSteps.Add(new VisualizationStep
                        {
                            DataState = (int[])currentDataSet.Clone(),
                            DelayMs = 200
                        });

                        if (stepValue < currentDataSet.Length - 1)
                        {
                            if (currentDataSet[stepValue] > currentDataSet[stepValue + 1])
                            {
                                // Swap elements
                                (currentDataSet[stepValue], currentDataSet[stepValue + 1]) = (currentDataSet[stepValue + 1], currentDataSet[stepValue]);

                                visualizationSteps.Add(new VisualizationStep
                                {
                                    DataState = (int[])currentDataSet.Clone(),
                                    DelayMs = 200
                                });
                            }
                        }
                    }
                }
            }
            else if (step is ConditionStep cs)
            {
                var parts = cs.Condition.Split(' ');
                var left = EvaluateExpression(parts[0], variables);
                var right = EvaluateExpression(parts[2], variables);
                var conditionMet = parts[1] switch
                {
                    "<" => left < right,
                    ">" => left > right,
                    "==" => left == right,
                    "!=" => left != right,
                    _ => false
                };

                visualizationSteps.Add(new VisualizationStep
                {
                    DataState = (int[])currentDataSet.Clone(),
                    DelayMs = 0
                });

                if (!conditionMet)
                {
                    break;
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