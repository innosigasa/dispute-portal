using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Dispute.Requests;

public record DisputeFilters(
    DisputeStatus? Status,
    DateTime? DateFrom,
    DateTime? DateTo,
    DisputeReasonCode? ReasonCode
);
