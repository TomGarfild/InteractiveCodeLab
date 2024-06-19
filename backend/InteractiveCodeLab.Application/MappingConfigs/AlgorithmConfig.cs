using System.Reflection;
using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Infrastructure.Models;
using Mapster;

namespace InteractiveCodeLab.Application.MappingConfigs;

public class AlgorithmConfig
{
    public static void Configure()
    {
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        TypeAdapterConfig<Algorithm, AlgorithmData>.NewConfig()
            .Map(dest => dest.Version, _ => 1)
            .Map(dest => dest.LastUpdated, _ => DateTime.UtcNow);
    }
}