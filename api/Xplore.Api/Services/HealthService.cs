using Xplore.Api.DTOs;
using Xplore.Api.Services.Interfaces;

namespace Xplore.Api.Services;

public sealed class HealthService : IHealthService
{
    public Task<HealthDto> GetHealthAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new HealthDto(true));
    }
}
