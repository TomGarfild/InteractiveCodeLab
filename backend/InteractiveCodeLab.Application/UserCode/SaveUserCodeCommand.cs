using InteractiveCodeLab.Domain.Models;
using MediatR;

namespace InteractiveCodeLab.Application.UserCode;

public record SaveUserCodeCommand(UserCodeKey Key, string Code) : IRequest;