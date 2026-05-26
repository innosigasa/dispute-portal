namespace DisputePortal.Application.Feature.Auth.Requests;

public record LoginRequest(string Email, string Password);
public record RefreshTokenRequest(string RefreshToken);
