using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Results;

namespace DisputePortal.Application.Feature.Dispute.Service;

public interface IDisputeService
{
    Task<RequestResult<DisputeDetailDto>> RaiseDisputeAsync(RaiseDisputeCommand cmd, CancellationToken ct);
    Task<RequestResult<PagedResult<DisputeListItemDto>>> GetCustomerDisputesAsync(DisputeFilters filters, int page, int pageSize, CancellationToken ct);
    Task<RequestResult<DisputeDetailDto>> GetDisputeDetailAsync(Guid id, CancellationToken ct);
    Task<RequestResult<DisputeSummaryStatsDto>> GetSummaryStatsAsync(CancellationToken ct);
}
