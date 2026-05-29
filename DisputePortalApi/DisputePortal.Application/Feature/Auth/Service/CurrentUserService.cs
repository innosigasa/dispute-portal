using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using DisputePortal.Application.Domain.Models;

namespace DisputePortal.Application.Feature.Auth.Service;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor accessor;

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        this.accessor = accessor;
    }

    private ClaimsPrincipal User => accessor.HttpContext!.User;

    public string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    public string Role => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    public Guid? CustomerId
    {
        get
        {
            var val = User.FindFirstValue(AppConstants.ClaimTypeCustomerId);
            return Guid.TryParse(val, out var id) ? id : null;
        }
    }

    public bool IsCustomer => Role == AppConstants.RoleCustomer;

    public bool IsAgent => Role == AppConstants.RoleAgent;
}
