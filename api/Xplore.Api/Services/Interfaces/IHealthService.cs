using Xplore.Api.DTOs;

namespace Xplore.Api.Services.Interfaces;

public interface IHealthService
{
    Task<HealthDto> GetHealthAsync(CancellationToken cancellationToken);
}
