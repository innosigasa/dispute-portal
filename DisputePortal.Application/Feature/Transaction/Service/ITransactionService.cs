using DisputePortal.Application.Common;
using DisputePortal.Application.Feature.Transaction.Requests;
using DisputePortal.Application.Feature.Transaction.Results;

namespace DisputePortal.Application.Feature.Transaction.Service;

public interface ITransactionService
{
    Task<RequestResult<PagedResult<TransactionListItemDto>>> GetPagedAsync(
        Guid? accountId, TransactionFilters filters, int page, int pageSize, CancellationToken ct);

    Task<RequestResult<TransactionDetailDto>> GetByIdAsync(Guid id, CancellationToken ct);
}
