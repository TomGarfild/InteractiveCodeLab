using ErrorOr;
using MediatR;

namespace InteractiveCodeLab.Application.Authentication.Registration;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<string>>;