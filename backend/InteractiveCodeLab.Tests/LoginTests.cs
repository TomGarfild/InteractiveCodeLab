using InteractiveCodeLab.Application.Authentication.Login;
using InteractiveCodeLab.Infrastructure;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using NSubstitute;

namespace InteractiveCodeLab.UnitTests;

public class LoginTests
{
    private readonly IRepository<UserData> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly LoginCommandHandler _handler;

    public LoginTests()
    {
        _userRepository = Substitute.For<IRepository<UserData>>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new LoginCommandHandler(_userRepository, _passwordHasher, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        _userRepository.GetOne(Arg.Any<UserByEmailSpecification>()).Returns((UserData)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Authentication.InvalidCredentials", result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "wrongpass");
        var userData = new UserData { Email = "test@example.com", PasswordHash = "correcthash" };
        _userRepository.GetOne(Arg.Any<UserByEmailSpecification>()).Returns(userData);
        _passwordHasher.VerifyPassword(command.Password, userData.PasswordHash).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal("Authentication.InvalidCredentials", result.FirstError.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnJwtToken_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        var userData = new UserData { Email = "test@example.com", PasswordHash = "correcthash" };
        _userRepository.GetOne(Arg.Any<UserByEmailSpecification>()).Returns(userData);
        _passwordHasher.VerifyPassword(command.Password, userData.PasswordHash).Returns(true);
        _jwtTokenGenerator.GenerateToken(userData).Returns("valid-jwt-token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal("valid-jwt-token", result.Value);
    }

    [Fact]
    public async Task Handle_ShouldCallGetOneMethodOfRepository_WithCorrectSpecification()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.Received(1).GetOne(Arg.Is<UserByEmailSpecification>(spec => spec.Email == command.Email));
    }
}