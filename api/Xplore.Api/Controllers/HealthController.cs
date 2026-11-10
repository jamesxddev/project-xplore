using Microsoft.AspNetCore.Mvc;
using Xplore.Api.DTOs;
using Xplore.Api.Services.Interfaces;

namespace Xplore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<HealthDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<HealthDto>>> Get(CancellationToken cancellationToken)
    {
        var data = await _healthService.GetHealthAsync(cancellationToken);
        return Ok(new ApiResponse<HealthDto>("ok", data));
    }
}
