namespace DisputePortal.Application.Domain.Entities;

public class AppUser
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
