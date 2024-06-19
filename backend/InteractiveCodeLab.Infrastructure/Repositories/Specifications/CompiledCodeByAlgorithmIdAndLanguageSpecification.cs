using System.Linq.Expressions;
using InteractiveCodeLab.Infrastructure.Models;

namespace InteractiveCodeLab.Infrastructure.Repositories.Specifications;

public record CompiledCodeByAlgorithmIdAndLanguageSpecification(string UserId, string AlgorithmId, string SelectedLanguage) : ISpecification<CompiledUserCodeData>
{
    public Expression<Func<CompiledUserCodeData, bool>> Build()
    {
        return userCode =>
            userCode.UserId == UserId &&
            userCode.AlgorithmId == AlgorithmId &&
            userCode.SelectedLanguage == SelectedLanguage;
    }
}