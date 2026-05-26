using DisputePortal.Application.Common;
using DisputePortal.Application.Feature.Account.Persistence;
using DisputePortal.Application.Feature.Account.Results;
using DisputePortal.Application.Feature.Auth.Service;

namespace DisputePortal.Application.Feature.Account.Service;

public class BankAccountService(
    IBankAccountRepository repo,
    ICurrentUserService currentUser) : IBankAccountService
{
    public async Task<RequestResult<IReadOnlyList<BankAccountDto>>> GetCustomerAccountsAsync(CancellationToken ct)
    {
        var customerId = currentUser.CustomerId!.Value;
        var accounts = await repo.GetByCustomerIdAsync(customerId, ct);
        return RequestResult<IReadOnlyList<BankAccountDto>>.Ok(accounts);
    }

    public async Task<RequestResult<BankAccountDto>> GetAccountByIdAsync(Guid accountId, CancellationToken ct)
    {
        var customerId = currentUser.CustomerId!.Value;
        var account = await repo.GetByIdAsync(accountId, customerId, ct);
        return account is null
            ? RequestResult<BankAccountDto>.NotFound($"Account {accountId} not found.")
            : RequestResult<BankAccountDto>.Ok(account);
    }
}
