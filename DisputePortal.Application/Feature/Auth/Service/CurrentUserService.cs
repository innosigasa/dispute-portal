using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DisputePortal.Application.Feature.Auth.Service;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    private ClaimsPrincipal User => accessor.HttpContext!.User;

    public string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    public string Role => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    public Guid? CustomerId
    {
        get
        {
            var val = User.FindFirstValue("customerId");
            return Guid.TryParse(val, out var id) ? id : null;
        }
    }
    public bool IsCustomer => Role == "customer";
    public bool IsAgent => Role == "agent";
}
