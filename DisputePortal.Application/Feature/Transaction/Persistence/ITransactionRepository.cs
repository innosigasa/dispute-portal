using DisputePortal.Application.Common;
using DisputePortal.Application.Feature.Transaction.Requests;
using DisputePortal.Application.Feature.Transaction.Results;

namespace DisputePortal.Application.Feature.Transaction.Persistence;

public interface ITransactionRepository
{
    Task<PagedResult<TransactionListItemDto>> GetPagedAsync(
        Guid customerId, Guid? accountId, TransactionFilters filters, int page, int pageSize, CancellationToken ct);

    Task<TransactionDetailDto?> GetByIdAsync(Guid id, Guid customerId, CancellationToken ct);

    Task SetDisputedAsync(Guid id, CancellationToken ct);
}
