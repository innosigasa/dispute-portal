namespace DisputePortal.Application.Feature.Auth.Service;

public interface ICurrentUserService
{
    string UserId { get; }
    string Role { get; }
    Guid? CustomerId { get; }
    bool IsCustomer { get; }
    bool IsAgent { get; }
}
