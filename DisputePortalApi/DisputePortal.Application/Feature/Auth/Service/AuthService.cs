using DisputePortal.Application.Domain.Models;
using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Auth.Requests;
using DisputePortal.Application.Feature.User.Persistence;
using FluentValidation;

namespace DisputePortal.Application.Feature.Auth.Service;

public class AuthService : IAuthService
{
    private readonly IUserRepository userRepo;
    private readonly JwtTokenService jwtService;
    private readonly IPasswordHasher passwordHasher;
    private readonly IValidator<LoginRequest> loginValidator;

    public AuthService(IUserRepository userRepo,
        JwtTokenService jwtService,
        IPasswordHasher passwordHasher,
        IValidator<LoginRequest> loginValidator)
    {
        this.userRepo = userRepo;
        this.jwtService = jwtService;
        this.passwordHasher = passwordHasher;
        this.loginValidator = loginValidator;
    }

    public async Task<RequestResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var validation = await loginValidator.ValidateAsync(request, ct);
        
        if (!validation.IsValid)
            return RequestResult<LoginResponse>.BadRequest(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var user = await userRepo.GetByEmailAsync(request.Email, ct);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return RequestResult<LoginResponse>.Unauthorized("Invalid email or password.");

        return RequestResult<LoginResponse>.Ok(await IssueTokensAsync(user, ct));
    }

    public async Task<RequestResult<LoginResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        var hash = JwtTokenService.HashToken(request.RefreshToken);
        var stored = await userRepo.GetRefreshTokenAsync(hash, ct);
        if (stored is null)
            return RequestResult<LoginResponse>.Unauthorized("Invalid or expired refresh token.");

        await userRepo.RevokeRefreshTokenAsync(hash, ct);
        return RequestResult<LoginResponse>.Ok(await IssueTokensAsync(stored.User, ct));
    }

    public async Task<RequestResult<object>> LogoutAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        var hash = JwtTokenService.HashToken(request.RefreshToken);
        await userRepo.RevokeRefreshTokenAsync(hash, ct);
        return RequestResult<object>.NoContent();
    }

    private async Task<LoginResponse> IssueTokensAsync(AppUserModel user, CancellationToken ct)
    {
        var accessToken = jwtService.GenerateAccessToken(user.Id.ToString(), user.Role, user.CustomerId);
        var (rawToken, hash, expiresAt) = jwtService.GenerateRefreshToken();

        await userRepo.AddRefreshTokenAsync(new RefreshTokenModel
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = hash,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        }, ct);

        return new LoginResponse(accessToken, rawToken, user.Role);
    }
}
