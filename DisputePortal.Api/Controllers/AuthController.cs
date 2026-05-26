using DisputePortal.Application.Feature.Auth.Requests;
using DisputePortal.Application.Feature.Auth.Service;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.Api.Controllers;

[Route("api/auth")]
public class AuthController(IAuthService service) : BaseController
{
    /// <summary>Authenticate and receive access + refresh tokens.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        => ToActionResult(await service.LoginAsync(request, ct));

    /// <summary>Exchange a valid refresh token for new tokens.</summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
        => ToActionResult(await service.RefreshAsync(request, ct));

    /// <summary>Revoke the current refresh token.</summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request, CancellationToken ct)
        => ToActionResult(await service.LogoutAsync(request, ct));
}
