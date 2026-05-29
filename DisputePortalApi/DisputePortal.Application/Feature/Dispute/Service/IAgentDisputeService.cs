using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Results;

namespace DisputePortal.Application.Feature.Dispute.Service;

public interface IAgentDisputeService
{
    Task<RequestResult<PagedResult<DisputeListItemDto>>> GetAllDisputesAsync(DisputeFilters filters, int page, int pageSize, CancellationToken ct);
    Task<RequestResult<DisputeDetailDto>> GetDisputeDetailAsync(Guid id, CancellationToken ct);
    Task<RequestResult<DisputeDetailDto>> UpdateStatusAsync(UpdateDisputeStatusCommand cmd, CancellationToken ct);
}
