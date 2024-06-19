using ErrorOr;
using InteractiveCodeLab.Application.Authentication.Registration;
using InteractiveCodeLab.Infrastructure;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using NSubstitute;

namespace InteractiveCodeLab.UnitTests;

public class RegisterTests
{
    private readonly IRepository<UserData> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly RegisterCommandHandler _handler;

    public RegisterTests()
    {
        _userRepository = Substitute.For<IRepository<UserData>>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new RegisterCommandHandler(_userRepository, _passwordHasher, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserAlreadyExists()
    {
        // Arrange
        var command = new RegisterCommand("first", "last", "test@example.com", "password");
        var existingUser = new UserData { Email = "test@example.com" };
        _userRepository.GetOne(Arg.Any<UserByEmailSpecification>()).Returns(existingUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("User already exists with email: test@example.com", result.FirstError.Description);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPasswordHashFails()
    {
        // Arrange
        var command = new RegisterCommand("first", "last", "test@example.com", "password");
        _userRepository.GetOne(Arg.Any<UserByEmailSpecification>()).Returns((UserData)null);
        var passwordHashResult = Error.Validation(code: "Hashing.Failed", description: "Password hashing failed");
        _passwordHasher.HashPassword(command.Password).Returns(passwordHashResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Hashing.Failed", result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_ShouldSaveUser_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var command = new RegisterCommand("first", "last", "test@example.com", "password");
        _userRepository.GetOne(Arg.Any<UserByEmailSpecification>()).Returns((UserData)null);
        _passwordHasher.HashPassword(command.Password).Returns("hashedpassword");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.Received(1).Upsert(Arg.Is<UserData>(user =>
            user.Email == command.Email &&
            user.FirstName == command.FirstName &&
            user.LastName == command.LastName &&
            user.PasswordHash == "hashedpassword"));
    }

    [Fact]
    public async Task Handle_ShouldReturnJwtToken_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var command = new RegisterCommand("first", "last", "test@example.com", "password");
        _userRepository.GetOne(Arg.Any<UserByEmailSpecification>()).Returns((UserData)null);
        _passwordHasher.HashPassword(command.Password).Returns("hashedpassword");
        var user = new UserData
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = "hashedpassword"
        };
        var jwtToken = "valid-jwt-token";
        _jwtTokenGenerator.GenerateToken(user).Returns(jwtToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(jwtToken, result.Value);
    }

    [Fact]
    public async Task Handle_ShouldCallGenerateToken_WithCorrectUserData()
    {
        // Arrange
        var command = new RegisterCommand("first", "last", "test@example.com", "password");
        _userRepository.GetOne(Arg.Any<UserByEmailSpecification>()).Returns((UserData)null);
        _passwordHasher.HashPassword(command.Password).Returns("hashedpassword");
        var user = new UserData
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = "hashedpassword"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _jwtTokenGenerator.Received(1).GenerateToken(Arg.Is<UserData>(u =>
            u.Email == command.Email &&
            u.FirstName == command.FirstName &&
            u.LastName == command.LastName &&
            u.PasswordHash == "hashedpassword"));
    }
}
