# Spec Delta

## Purpose

Lets the local Vite frontend call the API during development, while production cross-origin access stays limited to origins that are explicitly configured.

## ADDED Requirements

### Requirement: Development allows the local Vite origin

When the API runs in Development, it SHALL allow cross-origin reads and preflight from `http://localhost:5173`. The allow-origin response SHALL name that origin and SHALL NOT use `*`.

#### Scenario: Development browser request

- **WHEN** the API is running in Development
- **AND** a request includes `Origin: http://localhost:5173`
- **THEN** the response allows that origin to read the response
- **AND** the allow-origin value is `http://localhost:5173`

#### Scenario: Development preflight

- **WHEN** the API is running in Development
- **AND** a preflight `OPTIONS` request includes `Origin: http://localhost:5173`
- **THEN** the preflight succeeds for that origin

### Requirement: Production is not open to every origin

When the API runs outside Development, it SHALL NOT send `Access-Control-Allow-Origin: *`. An origin SHALL be allowed only when that origin is explicitly configured for the environment. The existing frontend SHALL keep resolving the API base URL from `VITE_API_BASE_URL` without a required frontend code change.

#### Scenario: Unconfigured origin is not granted access

- **WHEN** the API is running outside Development
- **AND** no allowed origin is configured for `https://evil.example`
- **AND** a request includes `Origin: https://evil.example`
- **THEN** the response does not allow that origin
- **AND** the response does not allow every origin

#### Scenario: Configured production origin is allowed

- **WHEN** the API is running outside Development
- **AND** `https://app.example` is configured as an allowed origin
- **AND** a request includes `Origin: https://app.example`
- **THEN** the response allows `https://app.example`

### Requirement: Allowed origins come from environment configuration

The allowed browser origins SHALL be supplied by environment-specific configuration, including environment variables, so a deployment can set them without changing code or rebuilding the image with embedded secrets.

#### Scenario: Origin list follows the environment

- **WHEN** Development configuration lists `http://localhost:5173` and the production configuration lists a different explicit origin or no origin
- **THEN** each running environment allows only the origins configured for that environment
