using System.Linq.Expressions;
using InteractiveCodeLab.Infrastructure.Models;

namespace InteractiveCodeLab.Infrastructure.Repositories.Specifications;

public interface ISpecification<TData> where TData : IData
{
    Expression<Func<TData, bool>> Build();
}