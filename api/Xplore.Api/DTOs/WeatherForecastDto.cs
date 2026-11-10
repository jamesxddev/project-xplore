namespace Xplore.Api.DTOs;

public sealed record WeatherForecastDto(DateOnly Date, int Temperature, string Summary);
