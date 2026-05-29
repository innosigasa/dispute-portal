using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Auth.Requests;

namespace DisputePortal.Application.Feature.Auth.Service;

public record LoginResponse
{
    public LoginResponse(string AccessToken, string RefreshToken, string Role)
    {
        this.AccessToken = AccessToken;
        this.RefreshToken = RefreshToken;
        this.Role = Role;
    }

    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public string Role { get; init; }

    public void Deconstruct(out string AccessToken, out string RefreshToken, out string Role)
    {
        AccessToken = this.AccessToken;
        RefreshToken = this.RefreshToken;
        Role = this.Role;
    }
}

public interface IAuthService
{
    Task<RequestResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<RequestResult<LoginResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken ct);
    Task<RequestResult<object>> LogoutAsync(RefreshTokenRequest request, CancellationToken ct);
}
