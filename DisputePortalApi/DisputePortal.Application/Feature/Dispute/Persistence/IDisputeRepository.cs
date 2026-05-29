using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Domain.Requests;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Results;

namespace DisputePortal.Application.Feature.Dispute.Persistence;

public interface IDisputeRepository
{
    Task<global::DisputePortal.Application.Domain.Models.DisputeModel> CreateAsync(
        global::DisputePortal.Application.Domain.Models.DisputeModel dispute, CancellationToken ct);

    Task<PagedResult<DisputeListItemDto>> GetCustomerDisputesAsync(
        Guid customerId, DisputeFilters filters, int page, int pageSize, CancellationToken ct);

    Task<DisputeDetailDto?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<PagedResult<DisputeListItemDto>> GetAllPagedAsync(
        DisputeFilters filters, int page, int pageSize, CancellationToken ct);

    Task<DisputeDetailDto?> UpdateStatusAsync(
        Guid id, DisputeStatus newStatus, string notes,
        string agentId, string agentRole, CancellationToken ct);

    Task<int> GetMaxReferenceSequenceForYearAsync(int year, CancellationToken ct);

    Task<DisputeSummaryStatsDto> GetSummaryStatsAsync(Guid customerId, CancellationToken ct);
}
