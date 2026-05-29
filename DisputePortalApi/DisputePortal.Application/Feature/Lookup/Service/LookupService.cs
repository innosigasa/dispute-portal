using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Lookup.Persistence;
using DisputePortal.Application.Feature.Lookup.Results;

namespace DisputePortal.Application.Feature.Lookup.Service;

public class LookupService : ILookupService
{
    private readonly ILookupRepository repo;

    public LookupService(ILookupRepository repo)
    {
        this.repo = repo;
    }

    public async Task<RequestResult<IReadOnlyList<DisputeReasonDto>>> GetDisputeReasonsAsync(CancellationToken ct)
        => RequestResult<IReadOnlyList<DisputeReasonDto>>.Ok(await repo.GetDisputeReasonsAsync(ct));

    public async Task<RequestResult<IReadOnlyList<TransactionCategoryDto>>> GetTransactionCategoriesAsync(CancellationToken ct)
        => RequestResult<IReadOnlyList<TransactionCategoryDto>>.Ok(await repo.GetTransactionCategoriesAsync(ct));
}
