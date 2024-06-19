using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using MediatR;

namespace InteractiveCodeLab.Application.UserCode;

public class SaveUserCodeCommandHandler : IRequestHandler<SaveUserCodeCommand>
{
    private readonly IRepository<UserCodeData> _userCodeRepository;

    public SaveUserCodeCommandHandler(IRepository<UserCodeData> userCodeRepository)
    {
        _userCodeRepository = userCodeRepository;
    }

    public async Task Handle(SaveUserCodeCommand request, CancellationToken cancellationToken)
    {
        var userCode = await _userCodeRepository
            .GetOne(new UserCodeByAlgorithmIdAndLanguageSpecification(request.Key.UserId, request.Key.AlgorithmId, request.Key.SelectedLanguage));

        var userCodeData = new UserCodeData
        {
            Id = userCode?.Id ?? Guid.NewGuid().ToString(),
            UserId = request.Key.UserId,
            AlgorithmId = request.Key.AlgorithmId,
            SelectedLanguage = request.Key.SelectedLanguage,
            Code = request.Code,
            LastUpdated = DateTime.UtcNow,
            Version = (userCode?.Version ?? 0) + 1 
        };

        await _userCodeRepository.Upsert(userCodeData);
    }
}
