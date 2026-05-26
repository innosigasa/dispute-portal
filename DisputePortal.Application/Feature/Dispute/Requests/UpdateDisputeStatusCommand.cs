using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Dispute.Requests;

public record UpdateDisputeStatusCommand(
    Guid DisputeId,
    DisputeStatus NewStatus,
    string Notes
);
