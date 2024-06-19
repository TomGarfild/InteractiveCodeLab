using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using InteractiveCodeLab.Visualizer;
using Mapster;
using MediatR;

namespace InteractiveCodeLab.Application.UserCode;

public class CompileUserCodeCommandHandler : IRequestHandler<CompileUserCodeCommand, bool>
{
    private readonly IMediator _mediator;
    private readonly IRepository<CompiledUserCodeData> _repository;
    
    public CompileUserCodeCommandHandler(IMediator mediator, IRepository<CompiledUserCodeData> repository)
    {
        _mediator = mediator;
        _repository = repository;
    }
    
    public async Task<bool> Handle(CompileUserCodeCommand request, CancellationToken cancellationToken)
    {
        var userCode = await _mediator.Send(new GetUserCodeQuery(request.Key), cancellationToken);

        var steps = CodeParser.Parse(request.Key.SelectedLanguage, userCode?.Code);

        var compiledUserCode = await _repository
            .GetOne(new CompiledCodeByAlgorithmIdAndLanguageSpecification(request.Key.UserId, request.Key.AlgorithmId, request.Key.SelectedLanguage));

        var compiledUserCodeData = new CompiledUserCodeData
        {
            Id = compiledUserCode?.Id ?? Guid.NewGuid().ToString(),
            UserId = request.Key.UserId,
            AlgorithmId = request.Key.AlgorithmId,
            SelectedLanguage = request.Key.SelectedLanguage,
            CompiledSuccessfully = true,
            VariableAssignmentSteps = steps.Where(s => s is VariableAssignmentStep).Select(s => s.Adapt<VariableAssignmentStepData>()).ToArray(),
            LoopSteps = steps.Where(s => s is LoopStep).Select(s => s.Adapt<LoopStepData>()).ToArray(),
            ConditionSteps = steps.Where(s => s is ConditionStep).Select(s => s.Adapt<ConditionStepData>()).ToArray(),
            LastUpdated = DateTime.UtcNow,
            Version = (compiledUserCode?.Version ?? 0) + 1
        };

        await _repository.Upsert(compiledUserCodeData);

        return true;
    }
}