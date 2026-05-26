using DisputePortal.Application.Common;
using DisputePortal.Application.Feature.Account.Results;

namespace DisputePortal.Application.Feature.Account.Service;

public interface IBankAccountService
{
    Task<RequestResult<IReadOnlyList<BankAccountDto>>> GetCustomerAccountsAsync(CancellationToken ct);
    Task<RequestResult<BankAccountDto>> GetAccountByIdAsync(Guid accountId, CancellationToken ct);
}
