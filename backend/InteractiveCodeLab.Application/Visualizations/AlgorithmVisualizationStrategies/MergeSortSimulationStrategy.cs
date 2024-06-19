using InteractiveCodeLab.Domain.Models;

namespace InteractiveCodeLab.Application.Visualizations.AlgorithmVisualizationStrategies;

public class MergeSortSimulationStrategy : IAlgorithmSimulationStrategy
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
            else if (step is FunctionStep fcs && fcs.FunctionName == "mergesort")
            {
                var start = EvaluateExpression(fcs.Arguments[0], variables);
                var end = EvaluateExpression(fcs.Arguments[1], variables);

                MergeSort(currentDataSet, start, end, visualizationSteps);
            }
        }

        return visualizationSteps;
    }

    private void MergeSort(int[] arr, int l, int r, List<VisualizationStep> visualizationSteps)
    {
        if (l < r)
        {
            int m = l + (r - l) / 2;

            MergeSort(arr, l, m, visualizationSteps);
            MergeSort(arr, m + 1, r, visualizationSteps);

            Merge(arr, l, m, r, visualizationSteps);
        }
    }

    private void Merge(int[] arr, int l, int m, int r, List<VisualizationStep> visualizationSteps)
    {
        int n1 = m - l + 1;
        int n2 = r - m;

        int[] L = new int[n1];
        int[] R = new int[n2];

        Array.Copy(arr, l, L, 0, n1);
        Array.Copy(arr, m + 1, R, 0, n2);

        int i = 0, j = 0;
        int k = l;

        while (i < n1 && j < n2)
        {
            if (L[i] <= R[j])
            {
                arr[k] = L[i];
                i++;
            }
            else
            {
                arr[k] = R[j];
                j++;
            }
            k++;

            visualizationSteps.Add(new VisualizationStep
            {
                DataState = (int[])arr.Clone(),
                DelayMs = 200
            });
        }

        while (i < n1)
        {
            arr[k] = L[i];
            i++;
            k++;

            visualizationSteps.Add(new VisualizationStep
            {
                DataState = (int[])arr.Clone(),
                DelayMs = 200
            });
        }

        while (j < n2)
        {
            arr[k] = R[j];
            j++;
            k++;

            visualizationSteps.Add(new VisualizationStep
            {
                DataState = (int[])arr.Clone(),
                DelayMs = 200
            });
        }
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
