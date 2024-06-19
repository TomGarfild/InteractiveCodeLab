using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Infrastructure.Models;
using Mapster;

namespace InteractiveCodeLab.Application.UserCode;

public static class CompiledUserCodeConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<VariableAssignmentStep, VariableAssignmentStepData>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Value, src => src.Value)
            .Map(dest => dest.VariableName, src => src.VariableName)
            .Map(dest => dest.Version, _ => 1)
            .Map(dest => dest.LastUpdated, _ => DateTime.UtcNow);
        TypeAdapterConfig<LoopStep, LoopStepData>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Iterable, src => src.Iterable)
            .Map(dest => dest.LoopVariable, src => src.LoopVariable)
            .Map(dest => dest.Steps, src => src.Steps)
            .Map(dest => dest.Version, _ => 1)
            .Map(dest => dest.LastUpdated, _ => DateTime.UtcNow);
        TypeAdapterConfig<ConditionStep, ConditionStepData>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Condition, src => src.Condition)
            .Map(dest => dest.Steps, src => src.Steps)
            .Map(dest => dest.Version, _ => 1)
            .Map(dest => dest.LastUpdated, _ => DateTime.UtcNow);
        
        TypeAdapterConfig<CompiledUserCodeData, CompiledUserCode>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Key, src => new UserCodeKey(src.UserId, src.AlgorithmId, src.SelectedLanguage))
            .Map(dest => dest.Steps, src => 
                src.VariableAssignmentSteps.Select(vas => vas.Adapt<VariableAssignmentStep>())
                    .Concat<Step>(src.LoopSteps.Select(ls => ls.Adapt<LoopStep>()))
                    .Concat(src.ConditionSteps.Select(cs => cs.Adapt<ConditionStep>()))
                    .OrderBy(step => step.Id).ToArray());
    }
}