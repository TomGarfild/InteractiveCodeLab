using ErrorOr;
using MediatR;

namespace InteractiveCodeLab.Application.Authentication.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<ErrorOr<string>>;