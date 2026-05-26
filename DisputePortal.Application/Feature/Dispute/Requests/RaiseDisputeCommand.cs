using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Dispute.Requests;

public record RaiseDisputeCommand(
    Guid TransactionId,
    DisputeReasonCode ReasonCode,
    string Comments
);
