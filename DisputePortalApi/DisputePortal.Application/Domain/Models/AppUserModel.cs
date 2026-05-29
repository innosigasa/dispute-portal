namespace DisputePortal.Application.Domain.Models;

public class AppUserModel
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty;
    
    public Guid? CustomerId { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<RefreshTokenModel> RefreshTokens { get; set; } = [];
}
