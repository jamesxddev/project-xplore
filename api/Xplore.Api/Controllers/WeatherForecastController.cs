using Microsoft.AspNetCore.Mvc;
using Xplore.Api.DTOs;
using Xplore.Api.Services.Interfaces;

namespace Xplore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public WeatherForecastController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<WeatherForecastDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<WeatherForecastDto>>>> Get(CancellationToken cancellationToken)
    {
        var data = await _weatherForecastService.GetForecastAsync(cancellationToken);
        return Ok(new ApiResponse<IReadOnlyList<WeatherForecastDto>>("ok", data));
    }
}
