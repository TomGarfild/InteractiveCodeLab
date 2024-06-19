using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using Mapster;

namespace InteractiveCodeLab.Application.Services;

public interface IAlgorithmsService
{
    public Task<IList<Algorithm>> GetAll();
    public Task<Algorithm> Get(string id);
    public Task Upsert(Algorithm algorithm);
}

public class AlgorithmsService : IAlgorithmsService
{
    private readonly IRepository<AlgorithmData> _algorithmRepository;

    public AlgorithmsService(IRepository<AlgorithmData> algorithmRepository)
    {
        _algorithmRepository = algorithmRepository;
    }

    public async Task<IList<Algorithm>> GetAll()
    {
        var algorithms = await _algorithmRepository.GetAll();
        return algorithms.Adapt<IList<Algorithm>>();
    }

    public async Task<Algorithm> Get(string id)
    {
        var algorithm = await _algorithmRepository.GetById(id);
        return algorithm.Adapt<Algorithm>();
    }

    public Task Upsert(Algorithm algorithm)
    {
        return _algorithmRepository.Upsert(algorithm.Adapt<AlgorithmData>());
    }
}