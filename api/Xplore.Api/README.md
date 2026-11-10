# Xplore API

ASP.NET Core Web API (.NET 8) for the Xplore frontend. It serves health, sample weather, and application info, and it is ready to run in a container. This repository change does not deploy to Google Cloud.

## Prerequisites

- .NET 8 SDK
- Docker, for the container workflow

## Run locally

From `api/Xplore.Api`:

```bash
dotnet restore
dotnet build
dotnet run
```

The API listens on `http://0.0.0.0:8080` unless `PORT` is set. `dotnet run` uses the Development environment, which enables Swagger at `/swagger` and allows the Vite origin `http://localhost:5173`.

The frontend already reads `VITE_API_BASE_URL`. `web/.env.example` points it at `http://localhost:8080`. No frontend change is required.

## Docker

From `api/Xplore.Api`:

```bash
docker build -t xplore-api .
docker run -p 8080:8080 xplore-api
```

The image publishes in Release, runs `Xplore.Api.dll` on the ASP.NET 8 runtime, and listens on `PORT` (default 8080) on all interfaces. Build output, git metadata, Visual Studio files, and `.env` files are excluded from the build context.

## Endpoints

All successful responses are JSON:

```json
{ "status": "ok", "data": {} }
```

| Method | Path | `data` |
| --- | --- | --- |
| GET | `/api/health` | `{ "healthy": true }` |
| GET | `/api/weatherforecast` | array of `{ "date", "temperature", "summary" }` |
| GET | `/api/info` | `{ "name": "Xplore API", "environment": "<environment name>" }` |

`temperature` is Celsius. These endpoints do not require authentication and do not use a database.

Swagger UI is available at `/swagger` only when `ASPNETCORE_ENVIRONMENT` is `Development`.

## Environment

| Variable | Purpose |
| --- | --- |
| `PORT` | HTTP port. Defaults to `8080`. |
| `ASPNETCORE_ENVIRONMENT` | `Development` enables Swagger and the local Vite origin. Any other value uses the base settings. |
| `CORS_ALLOWED_ORIGINS` | Optional comma-separated origin list. When set, it replaces `Cors:AllowedOrigins`. |

Development configuration allows `http://localhost:5173`. The base configuration allows no origins. The API never uses `Access-Control-Allow-Origin: *`.

Do not put secrets or credentials in the image. Pass deployment settings as environment variables.

## Google Cloud Run

Deploy later, outside this change:

1. Build the image from `api/Xplore.Api` and push it to the registry you use for Cloud Run.
2. Deploy that image. Leave `PORT` unset in the service so Cloud Run can assign it. The process reads `PORT` and serves HTTP inside the container. TLS terminates at Cloud Run.
3. Set `CORS_ALLOWED_ORIGINS` to the real frontend origin. Do not use `*`.
4. Do not bake secrets into the image. Use the service's environment or secret manager.

Rolling back that later deploy is a Cloud Run revision rollback. It does not require a change to this repository.
