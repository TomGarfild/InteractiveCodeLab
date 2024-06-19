using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using Mapster;
using MediatR;

namespace InteractiveCodeLab.Application.Visualizations;

public class CreateVisualizationCommandHandler : IRequestHandler<CreateVisualizationCommand, List<VisualizationStep>?>
{
    private readonly IRepository<CompiledUserCodeData> _compiledCodeRepository;
    private readonly IAlgorithmSimulationFactory _factory;

    public CreateVisualizationCommandHandler(IRepository<CompiledUserCodeData> compiledCodeRepository, IAlgorithmSimulationFactory factory)
    {
        _compiledCodeRepository = compiledCodeRepository;
        _factory = factory;
    }

    public async Task<List<VisualizationStep>?> Handle(CreateVisualizationCommand request, CancellationToken cancellationToken)
    {
        var compiledUserCodeData = await _compiledCodeRepository
            .GetOne(new CompiledCodeByAlgorithmIdAndLanguageSpecification(request.Key.UserId, request.Key.AlgorithmId,
                request.Key.SelectedLanguage));
        var compiledUserCode = compiledUserCodeData.Adapt<CompiledUserCode?>();
        var strategy = _factory.GetStrategy(compiledUserCode?.Key.AlgorithmId);
        return strategy.Simulate(compiledUserCode?.Steps ?? Array.Empty<Step>(), new Dictionary<string, int[]>() { {"arr", request.DataSet}});
    }
}