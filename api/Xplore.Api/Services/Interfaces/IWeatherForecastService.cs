using Xplore.Api.DTOs;

namespace Xplore.Api.Services.Interfaces;

public interface IWeatherForecastService
{
    Task<IReadOnlyList<WeatherForecastDto>> GetForecastAsync(CancellationToken cancellationToken);
}
