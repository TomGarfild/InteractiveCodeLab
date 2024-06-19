using ErrorOr;
using InteractiveCodeLab.Infrastructure;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using MediatR;

namespace InteractiveCodeLab.Application.Authentication.Registration;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<string>>
{
    private readonly IRepository<UserData> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler
        (IRepository<UserData> userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetOne(new UserByEmailSpecification(request.Email));

        if (existingUser is not null)
        {
            return Error.Conflict(description: $"User already exists with email: {request.Email}");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        if (passwordHash.IsError)
        {
            return passwordHash.Errors;
        }

        var user = new UserData
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = passwordHash.Value
        };

        await _userRepository.Upsert(user);
        var token = _jwtTokenGenerator.GenerateToken(user);

        return token;
    }
}