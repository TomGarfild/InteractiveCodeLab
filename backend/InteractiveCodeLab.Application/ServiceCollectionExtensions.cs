using InteractiveCodeLab.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using InteractiveCodeLab.Application.UserCode;
using InteractiveCodeLab.Application.Visualizations;

namespace InteractiveCodeLab.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddSingleton<IAlgorithmsService, AlgorithmsService>();
        services.AddSingleton<IAlgorithmSimulationFactory, AlgorithmSimulationFactory>();
        
        CompiledUserCodeConfig.Configure();

        return services;
    }
}