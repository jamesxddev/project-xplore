namespace Xplore.Api.DTOs;

public sealed record ApiResponse<T>(string Status, T Data);
