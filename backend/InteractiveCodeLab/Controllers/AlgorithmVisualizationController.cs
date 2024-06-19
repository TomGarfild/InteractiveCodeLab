using InteractiveCodeLab.Application.UserCode;
using InteractiveCodeLab.Application.Visualizations;
using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Models;
using InteractiveCodeLab.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveCodeLab.Controllers;

[Authorize]
[ApiController]
[Route("api/algorithms")]
public class AlgorithmVisualizationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public AlgorithmVisualizationController(IMediator mediator, ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _logger = loggerFactory.CreateLogger(nameof(AlgorithmVisualizationController));
    }

    [HttpPost("{algorithmId}/visualization")]
    public async Task<IActionResult> VisualizeAlgorithm([FromRoute] string algorithmId, [FromBody] VisualizationRequest visReq)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var dataSet = await _mediator.Send(new GetDataSetCommand(algorithmId, visReq.Regime, visReq.CustomData));

        await _mediator.Send(new CreateVisualizationCommand(new UserCodeKey(userId, algorithmId, visReq.SelectedLanguage), dataSet));

        return Ok();
    }

    private string? GetUserId()
        => User.FindFirst(claim => claim.Type == "id")?.Value;
}