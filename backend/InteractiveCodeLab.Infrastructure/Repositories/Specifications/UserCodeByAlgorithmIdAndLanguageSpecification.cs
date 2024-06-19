using System.Linq.Expressions;
using InteractiveCodeLab.Infrastructure.Models;

namespace InteractiveCodeLab.Infrastructure.Repositories.Specifications;

public record UserCodeByAlgorithmIdAndLanguageSpecification(string UserId, string AlgorithmId, string SelectedLanguage) : ISpecification<UserCodeData>
{
    public Expression<Func<UserCodeData, bool>> Build()
    {
        return userCode =>
            userCode.UserId == UserId &&
            userCode.AlgorithmId == AlgorithmId &&
            userCode.SelectedLanguage == SelectedLanguage;
    }
}