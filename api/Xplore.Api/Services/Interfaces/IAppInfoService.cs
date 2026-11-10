using Xplore.Api.DTOs;

namespace Xplore.Api.Services.Interfaces;

public interface IAppInfoService
{
    Task<AppInfoDto> GetInfoAsync(CancellationToken cancellationToken);
}
