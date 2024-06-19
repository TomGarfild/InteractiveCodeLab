using InteractiveCodeLab.Application.Services;
using InteractiveCodeLab.Application.UserCode;
using InteractiveCodeLab.Domain.Models;
using InteractiveCodeLab.Models.Dto;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveCodeLab.Controllers;

[Authorize]
[ApiController]
[Route("api/algorithms")]
public class AlgorithmsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAlgorithmsService _algorithmsService;
    private readonly ILogger<AlgorithmsController> _logger;

    public AlgorithmsController(IMediator mediator, IAlgorithmsService algorithmsService, ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _algorithmsService = algorithmsService;
        _logger = loggerFactory.CreateLogger<AlgorithmsController>();
    }

    [HttpGet("all-previews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AlgorithmPreviewDto>>> GetAll()
    {
        var algorithms = await _algorithmsService.GetAll();
        return Ok(algorithms.Adapt<List<AlgorithmPreviewDto>>());
    }

    [HttpGet("{algorithmId}/{selectedLanguage}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AlgorithmDto>> GetById(string algorithmId, string selectedLanguage)
    {
        var userId = GetUserId();

        if (userId is null)
        {
            return Unauthorized();
        }

        var algorithm = await _algorithmsService.Get(algorithmId);
        if (algorithm is null)
        {
            _logger.LogInformation("No algorithm with {id}", algorithmId);
            return NotFound();
        }

        var userCode = await _mediator.Send(new GetUserCodeQuery(new UserCodeKey(userId, algorithmId, selectedLanguage)));

        return Ok((algorithm, userCode ?? UserCode.Default).Adapt<AlgorithmDto>());
    }

    [HttpPost("upsert")]
    public async Task<IActionResult> Upsert(AlgorithmDto dto)
    {
        await _algorithmsService.Upsert(dto.Adapt<Algorithm>());

        return Ok();
    }

    private string? GetUserId()
        => User.FindFirst(claim => claim.Type == "id")?.Value;
}