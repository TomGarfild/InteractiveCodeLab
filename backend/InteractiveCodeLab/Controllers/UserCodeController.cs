using InteractiveCodeLab.Application.UserCode;
using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveCodeLab.Controllers;

[Authorize]
[ApiController]
[Route("api/user-code")]
public class UserCodeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AlgorithmsController> _logger;

    public UserCodeController(IMediator mediator, ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _logger = loggerFactory.CreateLogger<AlgorithmsController>();
    }

    [HttpPost("save/{algorithmId}")]
    public async Task<IActionResult> SaveCode([FromRoute] string algorithmId, [FromBody] AlgorithmCodeDto dto)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var command = new SaveUserCodeCommand(new UserCodeKey(userId, algorithmId, dto.SelectedLanguage), dto.Code);

        await _mediator.Send(command);

        return Ok();
    }

    [HttpPost("compile/{algorithmId}/{selectedLanguage}")]
    public async Task<IActionResult> Compile(string algorithmId, string selectedLanguage)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var command = new CompileUserCodeCommand(new UserCodeKey(userId, algorithmId, selectedLanguage));

        await _mediator.Send(command);

        return Ok();
    }


    [HttpGet("get/{algorithmId}/{selectedLanguage}")]
    public async Task<IActionResult> GetCode([FromRoute] string algorithmId, [FromRoute] string selectedLanguage)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var userCode = await _mediator.Send(new GetUserCodeQuery(new UserCodeKey(userId, algorithmId, selectedLanguage)));
        return Ok(userCode);
    }

    private string? GetUserId()
        => User.FindFirst(claim => claim.Type == "id")?.Value;
}