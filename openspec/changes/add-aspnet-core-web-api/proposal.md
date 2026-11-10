# Proposal

## Why

The project is a React + Vite frontend with no backend of its own. A small ASP.NET Core Web API is needed now so the app has a real HTTP service to call, packaged so it can run locally and later on Google Cloud Run, without adding a database or authentication yet.

## What Changes

- Add an ASP.NET Core Web API (.NET 8, C#) that the existing frontend can call. Preserve current frontend behavior and files.
- Expose three read-only endpoints: `GET /api/health`, `GET /api/weatherforecast`, and `GET /api/info`, using controllers, services, and DTOs with consistent responses and appropriate status codes.
- Enable Swagger / OpenAPI in the Development environment only.
- Add a production-oriented multi-stage Docker image that publishes in Release, listens on `0.0.0.0` and the `PORT` environment variable (default 8080), and can be built and run locally. Include a `.dockerignore` that keeps build outputs, git metadata, and local artifacts out of the image.
- Configure environment-specific CORS so the local Vite origin (`http://localhost:5173`) is allowed in development, and production does not use unrestricted CORS.
- Document prerequisites, local `dotnet` and Docker commands, the endpoints, environment configuration, and the remaining steps before a Cloud Run deploy.
- Do not deploy to Google Cloud, add a database, add authentication, or put secrets in the image.

## Capabilities

### New Capabilities

- `public-api`: Health, sample weather forecast, and application info over HTTP, with consistent responses and Development-only Swagger.
- `container-runtime`: A .NET 8 container that builds and runs locally and listens on the port Cloud Run provides.
- `cross-origin-access`: Environment-specific CORS that allows the local Vite origin and refuses an open production policy.

### Modified Capabilities

None. The project has no existing specs.

## Impact

- New backend project (intended location `api/Xplore.Api/`, adjusted only if the repo layout requires it), plus its Dockerfile, `.dockerignore`, and backend README.
- New HTTP surface: `/api/health`, `/api/weatherforecast`, `/api/info`. No existing API to break.
- New runtime dependency on the .NET 8 SDK for local development and the official .NET 8 SDK and ASP.NET 8 runtime images for Docker.
- The React + Vite app in `web/` already reads `VITE_API_BASE_URL` (`.env.example` points at `http://localhost:8080`). No frontend behavior change is required for this change.
- Deployment stays out of scope. The container must be ready to start on Cloud Run, but nothing is deployed.
