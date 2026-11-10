using Microsoft.AspNetCore.Mvc;
using Xplore.Api.DTOs;
using Xplore.Api.Services.Interfaces;

namespace Xplore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class InfoController : ControllerBase
{
    private readonly IAppInfoService _appInfoService;

    public InfoController(IAppInfoService appInfoService)
    {
        _appInfoService = appInfoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<AppInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AppInfoDto>>> Get(CancellationToken cancellationToken)
    {
        var data = await _appInfoService.GetInfoAsync(cancellationToken);
        return Ok(new ApiResponse<AppInfoDto>("ok", data));
    }
}
