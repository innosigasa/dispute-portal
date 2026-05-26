using DisputePortal.Application.Common;
using DisputePortal.Application.Feature.Lookup.Results;

namespace DisputePortal.Application.Feature.Lookup.Service;

public interface ILookupService
{
    Task<RequestResult<IReadOnlyList<DisputeReasonDto>>> GetDisputeReasonsAsync(CancellationToken ct);
    Task<RequestResult<IReadOnlyList<TransactionCategoryDto>>> GetTransactionCategoriesAsync(CancellationToken ct);
}
