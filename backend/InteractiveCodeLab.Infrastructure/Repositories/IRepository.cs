using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;

namespace InteractiveCodeLab.Infrastructure.Repositories;

public interface IRepository<TData> where TData : IData
{
    public Task<IList<TData>> GetAll();
    public Task<TData> GetById(string id);
    public Task Upsert(TData data);
    public Task<IList<TData>> Get(ISpecification<TData> spec);
    public Task<TData?> GetOne(ISpecification<TData> spec);
}