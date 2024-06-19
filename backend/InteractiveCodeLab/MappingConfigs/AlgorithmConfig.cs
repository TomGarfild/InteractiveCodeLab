using Mapster;
using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Models.Dto;

namespace InteractiveCodeLab.MappingConfigs;

public class AlgorithmConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<(string algId, string userId, AlgorithmCodeDto dto), UserCode>.NewConfig()
            .Map(dest => dest.Key.AlgorithmId, src => src.algId)
            .Map(dest => dest.Key.UserId, src => src.userId)
            .Map(dest => dest.Id, src => src.userId + src.algId + src.dto.SelectedLanguage)
            .Map(dest => dest.Code, src => src.dto.Code)
            .Map(dest => dest.Key.SelectedLanguage, src => src.dto.SelectedLanguage);

        TypeAdapterConfig<(Algorithm algo, UserCode code), AlgorithmDto>.NewConfig()
            .Map(dest => dest.Id, src => src.algo.Id)
            .Map(dest => dest.Title, src => src.algo.Title)
            .Map(dest => dest.Description, src => src.algo.Description)
            .Map(dest => dest.Test1, src => src.algo.Test1)
            .Map(dest => dest.Test2, src => src.algo.Test2)
            .Map(dest => dest.Test3, src => src.algo.Test3)
            .Map(dest => dest.SelectedLanguage, src => src.code.Key.SelectedLanguage)
            .Map(dest => dest.Code, src => src.code.Code);
    }
}