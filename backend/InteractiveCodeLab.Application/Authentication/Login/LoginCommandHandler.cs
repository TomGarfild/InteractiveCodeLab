using ErrorOr;
using InteractiveCodeLab.Infrastructure.Models;
using InteractiveCodeLab.Infrastructure.Repositories;
using InteractiveCodeLab.Infrastructure;
using InteractiveCodeLab.Infrastructure.Repositories.Specifications;
using MediatR;

namespace InteractiveCodeLab.Application.Authentication.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<string>>
{
    private readonly IRepository<UserData> _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IRepository<UserData> userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetOne(new UserByEmailSpecification(request.Email));

        return user is null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash)
            ? Error.Validation(code: "Authentication.InvalidCredentials", description: "Invalid credentials")
            : _jwtTokenGenerator.GenerateToken(user);
    }
}