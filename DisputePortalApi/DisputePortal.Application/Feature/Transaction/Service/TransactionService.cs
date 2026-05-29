using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Auth.Service;
using DisputePortal.Application.Feature.Transaction.Persistence;
using DisputePortal.Application.Feature.Transaction.Requests;
using DisputePortal.Application.Feature.Transaction.Results;

namespace DisputePortal.Application.Feature.Transaction.Service;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository repo;
    private readonly ICurrentUserService currentUser;

    public TransactionService(ITransactionRepository repo,
        ICurrentUserService currentUser)
    {
        this.repo = repo;
        this.currentUser = currentUser;
    }

    public async Task<RequestResult<PagedResult<TransactionListItemDto>>> GetPagedAsync(
        Guid? accountId, TransactionFilters filters, int page, int pageSize, CancellationToken ct)
    {
        var customerId = currentUser.CustomerId!.Value;
        var result = await repo.GetPagedAsync(customerId, accountId, filters, page, pageSize, ct);
        return RequestResult<PagedResult<TransactionListItemDto>>.Ok(result);
    }

    public async Task<RequestResult<TransactionDetailDto>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var customerId = currentUser.CustomerId!.Value;
        var result = await repo.GetByIdAsync(id, customerId, ct);
        return result is null
            ? RequestResult<TransactionDetailDto>.NotFound($"Transaction {id} not found.")
            : RequestResult<TransactionDetailDto>.Ok(result);
    }
}
