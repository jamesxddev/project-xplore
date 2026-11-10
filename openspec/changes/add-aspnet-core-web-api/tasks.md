# Tasks

## 1. Scaffold the API project

- [x] 1.1 Create `api/Xplore.sln` and `api/Xplore.Api` (`net8.0`, nullable reference types, implicit usings) and verify `dotnet build` from `api/Xplore.Api` succeeds.
- [x] 1.2 Add `bin/`, `obj/`, and `.vs/` to the root `.gitignore` and verify a local build does not leave those paths as files to commit.
- [x] 1.3 Add `Swashbuckle.AspNetCore` as the only extra package and verify `dotnet restore` succeeds.

## 2. Public API

- [x] 2.1 Add `ApiResponse<T>` plus the health, weather, and info DTOs and the weather model, and verify the camelCase names match `status`, `data`, `healthy`, `date`, `temperature`, `summary`, `name`, and `environment`.
- [x] 2.2 Add health, weather, and info services behind interfaces, generate forecasts in memory, and verify each service is registered for constructor injection in `Program.cs`.
- [x] 2.3 Add `HealthController`, `WeatherForecastController`, and `InfoController` on `api/[controller]` and verify `GET /api/health`, `GET /api/weatherforecast`, and `GET /api/info` return HTTP 200 with `"status": "ok"` and the spec payloads.
- [x] 2.4 Bind the host to `http://0.0.0.0` on `PORT` or 8080, set Development launch settings without HTTPS, and register Swagger only in Development at `/swagger`. Verify `/swagger` reaches the UI in Development and is not served outside Development.

## 3. Cross-origin access

- [x] 3.1 Configure `Cors:AllowedOrigins` with `http://localhost:5173` in Development and no origins in the base settings, and let `CORS_ALLOWED_ORIGINS` replace that list. Verify the policy never calls `AllowAnyOrigin`.
- [x] 3.2 Apply the CORS middleware and verify a Development request with `Origin: http://localhost:5173` allows that origin, a preflight succeeds, and an unconfigured origin outside Development is not allowed and is not `*`. Leave `web/` unchanged.

## 4. Container

- [x] 4.1 Add `api/Xplore.Api/Dockerfile` with the .NET 8 SDK build stage, ASP.NET 8 runtime stage, Release publish, `dotnet Xplore.Api.dll`, port 8080, and cleared `ASPNETCORE_URLS`, `ASPNETCORE_HTTP_PORTS`, and `ASPNETCORE_HTTPS_PORTS`. Verify `docker build -t xplore-api .` from `api/Xplore.Api` succeeds.
- [x] 4.2 Add `api/Xplore.Api/.dockerignore` for `bin/`, `obj/`, `.git/`, `.vs/`, user files, and `.env` files, and verify those paths are excluded from the image build context.
- [x] 4.3 Run the image and verify `GET http://localhost:8080/api/health` returns HTTP 200, and a container started with `PORT=9090` answers on 9090 inside the container.

## 5. Documentation

- [x] 5.1 Write `api/Xplore.Api/README.md` with prerequisites, the `dotnet` and Docker commands, the three endpoints, `PORT`, `ASPNETCORE_ENVIRONMENT`, and `CORS_ALLOWED_ORIGINS`, plus later Cloud Run steps. Verify the page does not deploy anything.

## 6. Integration check

- [x] 6.1 Run `dotnet build` from `api/Xplore.Api` and verify the three endpoints, Development-only Swagger, and the container health check together, with no frontend files changed and no database or authentication added.
