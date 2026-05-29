namespace DisputePortal.Application.Feature.Auth.Requests;

public record LoginRequest
{
    public LoginRequest(string Email, string Password)
    {
        this.Email = Email;
        this.Password = Password;
    }

    public string Email { get; init; }
    public string Password { get; init; }

    public void Deconstruct(out string Email, out string Password)
    {
        Email = this.Email;
        Password = this.Password;
    }
}

public record RefreshTokenRequest
{
    public RefreshTokenRequest(string RefreshToken)
    {
        this.RefreshToken = RefreshToken;
    }

    public string RefreshToken { get; init; }

    public void Deconstruct(out string RefreshToken)
    {
        RefreshToken = this.RefreshToken;
    }
}
