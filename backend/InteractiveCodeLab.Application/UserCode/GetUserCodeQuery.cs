using InteractiveCodeLab.Domain.Models;
using MediatR;

namespace InteractiveCodeLab.Application.UserCode;

public record GetUserCodeQuery(UserCodeKey Key) : IRequest<Domain.Models.UserCode?>;