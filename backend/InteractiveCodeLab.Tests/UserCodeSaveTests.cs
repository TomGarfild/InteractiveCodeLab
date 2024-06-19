using InteractiveCodeLab.Application.UserCode;
using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using NSubstitute;

namespace InteractiveCodeLab.UnitTests;

public class UserCodeSaveTests
{
    private readonly IRepository<UserCodeData> _userCodeRepository;
    private readonly SaveUserCodeCommandHandler _handler;

    public UserCodeSaveTests()
    {
        _userCodeRepository = Substitute.For<IRepository<UserCodeData>>();
        _handler = new SaveUserCodeCommandHandler(_userCodeRepository);
    }

    [Fact]
    public async Task Handle_ShouldSaveNewUserCode_WhenUserCodeDoesNotExist()
    {
        // Arrange
        var command = new SaveUserCodeCommand(new UserCodeKey("userid", "bubble-sort", "python"), "//code");
        _userCodeRepository.GetOne(Arg.Any<UserCodeByAlgorithmIdAndLanguageSpecification>()).Returns((UserCodeData)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userCodeRepository.Received(1).Upsert(Arg.Is<UserCodeData>(data =>
            data.UserId == command.Key.UserId &&
            data.AlgorithmId == command.Key.AlgorithmId &&
            data.SelectedLanguage == command.Key.SelectedLanguage &&
            data.Code == command.Code &&
            data.Version == 1));
    }

    [Fact]
    public async Task Handle_ShouldUpdateExistingUserCode_WhenUserCodeExists()
    {
        // Arrange
        var existingUserCode = new UserCodeData
        {
            Id = "existing-id",
            UserId = "userid",
            AlgorithmId = "bubble-sort",
            SelectedLanguage = "C#",
            Code = "old code",
            Version = 1
        };
        var command = new SaveUserCodeCommand(new UserCodeKey("userid", "bubble-sort", "C#"), "//code");
        _userCodeRepository.GetOne(Arg.Any<UserCodeByAlgorithmIdAndLanguageSpecification>()).Returns(existingUserCode);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userCodeRepository.Received(1).Upsert(Arg.Is<UserCodeData>(data =>
            data.Id == existingUserCode.Id &&
            data.UserId == command.Key.UserId &&
            data.AlgorithmId == command.Key.AlgorithmId &&
            data.SelectedLanguage == command.Key.SelectedLanguage &&
            data.Code == command.Code &&
            data.Version == existingUserCode.Version + 1));
    }

    [Fact]
    public async Task Handle_ShouldSetLastUpdatedToUtcNow_WhenSavingUserCode()
    {
        // Arrange
        var command = new SaveUserCodeCommand(new UserCodeKey("userid", "bubble-sort", "C#"), "//code");
        _userCodeRepository.GetOne(Arg.Any<UserCodeByAlgorithmIdAndLanguageSpecification>()).Returns((UserCodeData)null);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userCodeRepository.Received(1).Upsert(Arg.Is<UserCodeData>(data =>
            data.LastUpdated.Kind == DateTimeKind.Utc &&
            (DateTime.UtcNow - data.LastUpdated).TotalSeconds < 5)); // Allowing for small delay
    }
}
