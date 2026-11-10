using Xplore.Api.DTOs;
using Xplore.Api.Models;
using Xplore.Api.Services.Interfaces;

namespace Xplore.Api.Services;

public sealed class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

    public Task<IReadOnlyList<WeatherForecastDto>> GetForecastAsync(CancellationToken cancellationToken)
    {
        var forecast = Enumerable.Range(1, 5)
            .Select(index =>
            {
                var model = new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]);

                return new WeatherForecastDto(model.Date, model.Temperature, model.Summary);
            })
            .ToArray();

        return Task.FromResult<IReadOnlyList<WeatherForecastDto>>(forecast);
    }
}
