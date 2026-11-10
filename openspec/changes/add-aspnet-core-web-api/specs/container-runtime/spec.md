# Spec Delta

## Purpose

Packages the API so a container can be built and run locally, listen on the port provided at startup, and start on Cloud Run without embedded secrets.

## ADDED Requirements

### Requirement: Listen on all interfaces and the configured port

The hosted API SHALL accept HTTP connections on all interfaces (`0.0.0.0`). It SHALL listen on the port given by the `PORT` environment variable when that variable is set to a valid port, and SHALL listen on port 8080 when `PORT` is unset. It SHALL NOT bind only to the loopback interface.

#### Scenario: Default port

- **WHEN** the API process starts with `PORT` unset
- **THEN** it accepts HTTP connections on port 8080 on all interfaces

#### Scenario: Port override

- **WHEN** the API process starts with `PORT` set to `9090`
- **THEN** it accepts HTTP connections on port 9090
- **AND** it does not require clients inside the container to use port 8080

### Requirement: Runnable container

The API SHALL be buildable into an image that runs the published Release application. The running container SHALL NOT require a software-development kit at runtime. Starting the container with host port 8080 mapped to container port 8080 SHALL make the public API reachable on the host.

#### Scenario: Local container serves health

- **WHEN** the image is built and a container is started with host port 8080 mapped to container port 8080 and `PORT` unset or set to `8080`
- **THEN** `GET http://localhost:8080/api/health` returns HTTP 200

#### Scenario: Container starts without a development kit

- **WHEN** the container process starts
- **THEN** it runs the published application
- **AND** it does not require the .NET SDK in the running container

### Requirement: Cloud Run compatible HTTP

The container SHALL serve plain HTTP inside the container and SHALL start successfully when the platform sets `PORT`. Deployment to Google Cloud SHALL NOT be part of this capability.

#### Scenario: Platform-assigned port

- **WHEN** a container is started with `PORT` set to a platform-assigned port
- **THEN** the API accepts HTTP on that port inside the container
- **AND** the process stays running without a localhost-only bind

### Requirement: Image excludes local artifacts and secrets

The image build SHALL NOT include repository `bin/` or `obj/` outputs, `.git/`, `.vs/`, other local development artifacts, or secrets and credentials.

#### Scenario: Clean image contents

- **WHEN** the image is built from the repository
- **THEN** the image does not contain git metadata, Visual Studio local files, local build outputs, or secret material
