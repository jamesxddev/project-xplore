# Design

## Context

The repository is a React 19 + Vite app under `web/`. It already resolves its API base URL from `VITE_API_BASE_URL`, and `web/.env.example` points that variable at `http://localhost:8080`. There is no .NET project, solution, or Docker setup. See `proposal.md` for why this change exists, and the delta specs for the behavior the design has to satisfy.

The story's layout (`api/Xplore.Api`) matches the repo: frontend and backend stay siblings. Docker is built from the API project directory, so the frontend and `node_modules` never enter the image context.

## Goals / Non-Goals

**Goals:**

- One ASP.NET Core Web API project that serves the three read-only endpoints through controllers, services, and DTOs.
- The same listen address for `dotnet run` and the container: `http://0.0.0.0` on `PORT`, or 8080 when `PORT` is unset.
- A Release image that runs the published DLL on the ASP.NET 8 runtime, with Swagger limited to Development.
- CORS that names explicit origins and cannot fall through to `*`.

**Non-Goals:**

- Database, authentication, HTTPS inside the process, and any Google Cloud deploy.
- Frontend code changes, extra NuGet packages beyond OpenAPI, and an automated test project.
- Custom middleware. Built-in CORS and Swagger middleware are enough, so there is no empty `Middleware/` folder to commit.

## Decisions

### 1. Project location and shape

Add `api/Xplore.Api/` (`net8.0`, nullable reference types, implicit usings) and `api/Xplore.sln`. Local `dotnet restore`, `dotnet build`, and `dotnet run` run from `api/Xplore.Api`. The root `.gitignore` gains `bin/`, `obj/`, and `.vs/` so build outputs stay untracked. No other existing file needs to change.

Controllers, one per resource, use `[Route("api/[controller]")]`:

- `HealthController` → `GET /api/health`
- `WeatherForecastController` → `GET /api/weatherforecast`
- `InfoController` → `GET /api/info`

Each controller takes an interface via the constructor and returns `ActionResult<ApiResponse<T>>`. Services are registered in `Program.cs`. Weather generation is in-memory and asynchronous at the boundary (`Task`), with a small `WeatherForecast` model mapped to a DTO. No request bodies are required.

**Alternative:** Minimal APIs in `Program.cs`. Rejected because the story requires controllers and a service boundary.

**Alternative:** Put the API at the repo root. Rejected because `web/` is already the frontend home and a root-level Docker context would copy the frontend unless heavily ignored.

### 2. Response JSON

One generic envelope, serialized with the default camelCase policy:

```json
{ "status": "ok", "data": {} }
```

| Endpoint | `data` |
| --- | --- |
| `/api/health` | `{ "healthy": true }` |
| `/api/weatherforecast` | non-empty array of `{ "date", "temperature", "summary" }` |
| `/api/info` | `{ "name": "Xplore API", "environment": "<IHostEnvironment.EnvironmentName>" }` |

`date` is an ISO-8601 calendar date. `temperature` is an integer in Celsius. `name` and `environment` are plain strings. Info never copies configuration secrets. Swagger response types use the same envelope so the Development UI matches the wire format.

### 3. Listen address

`Program.cs` always sets the server URLs to `http://0.0.0.0:{port}`, where `port` is `PORT` when it is a valid integer and `8080` otherwise. `Properties/launchSettings.json` sets `ASPNETCORE_ENVIRONMENT=Development` and the same HTTP URL, and does not enable an HTTPS profile.

The ASP.NET 8 runtime image sets `ASPNETCORE_HTTP_PORTS=8080`. The Dockerfile clears `ASPNETCORE_URLS`, `ASPNETCORE_HTTP_PORTS`, and `ASPNETCORE_HTTPS_PORTS` so that image default cannot pin the process to 8080 or loopback after Cloud Run injects `PORT`. The app does not call `UseHttpsRedirection`.

**Alternative:** A shell entrypoint that rewrites `ASPNETCORE_URLS`. Rejected because the C# host covers both `dotnet run` and the container with one path.

### 4. OpenAPI only in Development

Reference `Swashbuckle.AspNetCore`. Register the generator and Swagger UI only when `IsDevelopment()` is true. The UI route prefix is `swagger`, so `/swagger` redirects to `/swagger/index.html`. Outside Development those middleware calls are omitted, and `/swagger` is unmapped. This is the only added package.

### 5. CORS from configuration

Configuration section `Cors:AllowedOrigins` (string array):

- `appsettings.Development.json` lists `http://localhost:5173`.
- `appsettings.json` lists no origins.

When `CORS_ALLOWED_ORIGINS` is set, its comma-separated values replace the configured list. That is the Cloud Run setting. The policy uses `WithOrigins` for a non-empty list, plus the headers and methods needed for GET and preflight. It never calls `AllowAnyOrigin`. An empty list uses a policy that allows no origin. Credentials are not enabled.

`web/` stays unchanged. `VITE_API_BASE_URL` is already the frontend base URL.

### 6. Image

`api/Xplore.Api/Dockerfile`, context `api/Xplore.Api`:

1. `mcr.microsoft.com/dotnet/sdk:8.0` restores and `dotnet publish -c Release`.
2. `mcr.microsoft.com/dotnet/aspnet:8.0` copies the publish output, exposes 8080, and runs `dotnet Xplore.Api.dll`.

`api/Xplore.Api/.dockerignore` excludes `bin/`, `obj/`, `.git/`, `.vs/`, user files, and `.env` files. No secrets are copied into the image. `ASPNETCORE_ENVIRONMENT` is not baked as a secret; Cloud Run can set it. Local commands, from `api/Xplore.Api`:

```bash
docker build -t xplore-api .
docker run -p 8080:8080 xplore-api
```

### 7. Documentation

`api/Xplore.Api/README.md` covers the .NET 8 SDK and Docker prerequisites, the `dotnet` and Docker commands above, the three endpoints and the JSON envelope, `PORT`, `ASPNETCORE_ENVIRONMENT`, and `CORS_ALLOWED_ORIGINS`, and the remaining Cloud Run steps: deploy the image later, let the platform set `PORT`, set allowed origins explicitly, and terminate TLS at the platform. Nothing in this change deploys.

## Risks / Trade-offs

- [Runtime image port defaults override `UseUrls`] → Clear `ASPNETCORE_URLS` and the HTTP/HTTPS port variables in the Dockerfile, then confirm `PORT=9090` is the port that answers inside the container.
- [`GET /swagger` is a redirect to `/swagger/index.html`] → Treat a redirect to the UI as success in Development, and confirm the route is absent outside Development.
- [An empty origin list is implemented with `AllowAnyOrigin` by mistake] → The policy builder must branch: named origins, or no origin. Do not register a wildcard fallback.
- [Port 8080 is already taken on the host] → Document the fixed local port; do not add a second listen URL.
- [Building from the repo root ignores this Dockerfile] → README states the working directory is `api/Xplore.Api`.

## Migration Plan

The change is additive. Implementing it adds `api/` and extends the root `.gitignore`. Rolling back means removing `api/` and restoring `.gitignore`. There is no data to migrate and no deployment to reverse.

Later Cloud Run rollout, not part of this change: build the image, deploy it without embedding secrets, set `CORS_ALLOWED_ORIGINS` to the real frontend origin, and leave `PORT` to the platform. Rollback of that future deploy is a platform revision rollback, independent of this repository change.
