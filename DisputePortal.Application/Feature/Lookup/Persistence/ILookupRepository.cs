using DisputePortal.Application.Feature.Lookup.Results;

namespace DisputePortal.Application.Feature.Lookup.Persistence;

public interface ILookupRepository
{
    Task<IReadOnlyList<DisputeReasonDto>> GetDisputeReasonsAsync(CancellationToken ct);
    Task<IReadOnlyList<TransactionCategoryDto>> GetTransactionCategoriesAsync(CancellationToken ct);
}
