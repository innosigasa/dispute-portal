using DisputePortal.Application.Feature.Auth.Requests;
using DisputePortal.Application.Feature.Auth.Service;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.WebApi.Controllers;

//[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IAuthService service;

    public AuthController(IAuthService service)
    {
        this.service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        => ToActionResult(await service.LoginAsync(request, ct));

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
        => ToActionResult(await service.RefreshAsync(request, ct));

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request, CancellationToken ct)
        => ToActionResult(await service.LogoutAsync(request, ct));
}
