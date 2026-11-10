**Story:**
# Task: Add ASP.NET Core Web API (.NET 8) with Docker Support

I want to extend my existing project by adding a backend application using **ASP.NET Core Web API (.NET 8)** and containerize it using Docker.

## 1. First, Analyze the Existing Project

Before making any changes:

- Inspect the existing repository structure.
- Identify the existing frontend application and its framework.
- Check the existing naming conventions, configurations, and coding standards.
- Preserve all existing frontend functionality.
- Do not overwrite, remove, or unnecessarily refactor existing files.
- Determine the most appropriate location for the backend project.

## 2. Backend Technology Stack

Create a backend application with the following technologies:

- ASP.NET Core Web API (.NET 8)
- C#
- RESTful API architecture
- Dependency Injection
- Controllers and Services
- DTOs (Data Transfer Objects)
- Swagger / OpenAPI
- Docker

## 3. Backend Project Structure

Create a clean and maintainable structure:

```text
api/
└── Xplore.Api/
    ├── Controllers/
    │   └── WeatherForecastController.cs
    ├── Services/
    │   └── Interfaces/
    ├── DTOs/
    ├── Models/
    ├── Middleware/
    ├── Properties/
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── Dockerfile
    └── Xplore.Api.csproj
```

Adjust the project location if the existing repository structure suggests a better approach.

## 4. API Implementation

Implement the following initial endpoints:

- GET `/api/health` – Return API health status.
- GET `/api/weatherforecast` – Return sample weather forecast data.
- GET `/api/info` – Return basic application information.

Requirements:

- Use Controllers rather than putting all endpoints in Program.cs.
- Follow separation of concerns.
- Use constructor-based dependency injection.
- Use asynchronous methods where appropriate.
- Return appropriate HTTP status codes.
- Include basic request and response DTOs where appropriate.
- Implement consistent API response conventions.
- Enable Swagger in the Development environment.

## 5. Docker Configuration

Create a production-ready multi-stage Dockerfile.

Requirements:

- Use the official Microsoft .NET 8 SDK image for building.
- Use the official ASP.NET 8 runtime image for execution.
- Separate build and runtime stages.
- Publish the application in Release configuration.
- Expose port 8080.
- Configure the application to listen on `0.0.0.0:8080`.
- Support the PORT environment variable used by Google Cloud Run.
- Run the application using the published DLL.
- Keep the final image as small as reasonably practical.

Also create a `.dockerignore` file to exclude unnecessary files such as:

- bin/
- obj/
- .git/
- .vs/
- local development artifacts.

## 6. Local Development Configuration

Ensure the backend can run both ways:

**Without Docker:**

```bash
dotnet restore
dotnet build
dotnet run
```

**With Docker:**

```bash
docker build -t xplore-api .
docker run -p 8080:8080 xplore-api
```

Verify that the API is accessible locally.

## 7. Frontend Integration Preparation

Prepare the backend for integration with the existing React + Vite frontend.

Requirements:

- Configure CORS using environment-specific configuration.
- Allow the local Vite development origin (typically `http://localhost:5173`).
- Do not use unrestricted CORS in production.
- Make the API base URL configurable through environment variables on the frontend.
- Do not introduce unnecessary frontend changes.

## 8. Google Cloud Run Compatibility

Prepare the backend for deployment to Google Cloud Run.

Requirements:

- Listen on the port provided by the PORT environment variable.
- Support HTTP traffic inside the container.
- Do not hardcode localhost as the production listening address.
- Use environment variables for deployment-specific settings.
- Do not include secrets or credentials in the Docker image.
- Ensure the container can start successfully in Cloud Run.

Do not deploy anything to Google Cloud yet.

## 9. Documentation

Create or update the backend README documentation covering:

- Prerequisites.
- How to run locally.
- How to build the Docker image.
- How to run the Docker container.
- Available API endpoints.
- Environment configuration.
- Future Google Cloud Run deployment instructions.

## 10. Important Development Rules

- Follow existing repository conventions.
- Keep the implementation simple and maintainable.
- Avoid unnecessary packages and dependencies.
- Do not introduce a database yet.
- Do not implement authentication yet.
- Do not modify the existing frontend unless necessary.
- Ensure all code compiles successfully.
- Fix any build errors introduced by the implementation.

## 11. Expected Deliverables

After implementation, provide:

1. Summary of files created and modified.
2. Final backend folder structure.
3. Explanation of the Docker configuration.
4. Commands to run the API locally.
5. Commands to build and run the Docker container.
6. List of implemented API endpoints.
7. Confirmation that the project builds successfully.
8. Any remaining steps required before Google Cloud Run deployment.

**Important:** Analyze the existing repository first, then implement the backend. Do not simply generate sample code without integrating it into the existing project.