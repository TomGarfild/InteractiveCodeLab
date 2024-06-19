using ErrorOr;
using InteractiveCodeLab.Application.Authentication.Login;
using InteractiveCodeLab.Application.Authentication.Registration;
using InteractiveCodeLab.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveCodeLab.Controllers;

[Route("api/user")]
[AllowAnonymous]
public class UserController : ControllerBase
{
    private readonly ISender _mediator;

    public UserController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = request.Adapt<RegisterCommand>();
        var result = await _mediator.Send(command);

        return result.Match<IActionResult>(Ok, BadRequest);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = request.Adapt<LoginCommand>();
        var result = await _mediator.Send(command);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}