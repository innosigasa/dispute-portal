using DisputePortal.Application.Feature.Account.Results;

namespace DisputePortal.Application.Feature.Account.Persistence;

public interface IBankAccountRepository
{
    Task<IReadOnlyList<BankAccountDto>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct);
    Task<BankAccountDto?> GetByIdAsync(Guid id, Guid customerId, CancellationToken ct);
}
