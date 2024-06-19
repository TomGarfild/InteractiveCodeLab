using InteractiveCodeLab.Application.Services;
using InteractiveCodeLab.Domain.Models;
using MediatR;

namespace InteractiveCodeLab.Application.Visualizations;

public class GetDataSetCommandHandler : IRequestHandler<GetDataSetCommand, int[]>
{
    private readonly IAlgorithmsService _algorithmsService;
    public GetDataSetCommandHandler(IAlgorithmsService algorithmsService)
    {
        _algorithmsService = algorithmsService;
    }
    
    public async Task<int[]> Handle(GetDataSetCommand request, CancellationToken cancellationToken)
    {
        var algorithm = await _algorithmsService.Get(request.AlgorithmId);
        switch (request.Regime)
        {
            case VisualizationRegime.CustomInput:
                return request.CustomDataSet ?? Array.Empty<int>();
            case VisualizationRegime.TestSample1:
                return algorithm.Test1;
            case VisualizationRegime.TestSample2:
                return algorithm.Test2;
            case VisualizationRegime.TestSample3:
                return algorithm.Test3;
            case VisualizationRegime.LargeDataSet:
                return GenerateLargeDataSet(100);
            default:
                return Array.Empty<int>();
        }
    }
    
    private static int[] GenerateLargeDataSet(int size)
    {
        var random = new Random();
        var largeDataSet = Enumerable.Range(1, size)
            .Select(_ => random.Next(0, 10000))
            .ToArray();

        return largeDataSet;
    }
}