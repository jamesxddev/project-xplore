using Xplore.Api.DTOs;
using Xplore.Api.Services.Interfaces;

namespace Xplore.Api.Services;

public sealed class AppInfoService : IAppInfoService
{
    private readonly IHostEnvironment _environment;

    public AppInfoService(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public Task<AppInfoDto> GetInfoAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new AppInfoDto("Xplore API", _environment.EnvironmentName));
    }
}
