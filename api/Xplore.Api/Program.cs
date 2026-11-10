using Xplore.Api.Services;
using Xplore.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var port = builder.Configuration["PORT"];
if (string.IsNullOrWhiteSpace(port) || !int.TryParse(port, out var parsedPort) || parsedPort is < 1 or > 65535)
{
    port = "8080";
}

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}

builder.Services.AddSingleton<IHealthService, HealthService>();
builder.Services.AddSingleton<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddSingleton<IAppInfoService, AppInfoService>();

var origins = ResolveAllowedOrigins(builder.Configuration);
const string corsPolicy = "Frontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        if (origins.Length == 0)
        {
            policy.SetIsOriginAllowed(_ => false);
        }
        else
        {
            policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
    });
}

app.UseCors(corsPolicy);
app.MapControllers();
app.Run();

static string[] ResolveAllowedOrigins(IConfiguration configuration)
{
    var fromEnvironment = configuration["CORS_ALLOWED_ORIGINS"];
    if (!string.IsNullOrWhiteSpace(fromEnvironment))
    {
        return fromEnvironment.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    return configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
}
