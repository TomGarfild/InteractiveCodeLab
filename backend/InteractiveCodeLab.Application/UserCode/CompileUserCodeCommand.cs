using InteractiveCodeLab.Domain.Models;
using MediatR;

namespace InteractiveCodeLab.Application.UserCode;

public record CompileUserCodeCommand(UserCodeKey Key) : IRequest<bool>;