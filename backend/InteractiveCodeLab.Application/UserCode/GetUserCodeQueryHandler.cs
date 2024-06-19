using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using Mapster;
using MediatR;

namespace InteractiveCodeLab.Application.UserCode;

public class GetUserCodeQueryHandler : IRequestHandler<GetUserCodeQuery, Domain.Models.UserCode?>
{
    private readonly IRepository<UserCodeData> _userCodeRepository;

    public GetUserCodeQueryHandler(IRepository<UserCodeData> userCodeRepository)
    {
        _userCodeRepository = userCodeRepository;
    }

    public async Task<Domain.Models.UserCode?> Handle(GetUserCodeQuery request, CancellationToken cancellationToken)
    {
        var userCodeData = await _userCodeRepository
            .GetOne(new UserCodeByAlgorithmIdAndLanguageSpecification(request.Key.UserId, request.Key.AlgorithmId, request.Key.SelectedLanguage));

        return userCodeData.Adapt<Domain.Models.UserCode?>();
    }
}