using DisputePortal.Application.Common;
using DisputePortal.Application.Feature.Auth.Requests;

namespace DisputePortal.Application.Feature.Auth.Service;

public record LoginResponse(string AccessToken, string RefreshToken, string Role);

public interface IAuthService
{
    Task<RequestResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<RequestResult<LoginResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken ct);
    Task<RequestResult<object>> LogoutAsync(RefreshTokenRequest request, CancellationToken ct);
}
