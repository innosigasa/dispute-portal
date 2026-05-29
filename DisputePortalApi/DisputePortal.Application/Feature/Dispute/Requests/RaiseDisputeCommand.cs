using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Dispute.Requests;

public record RaiseDisputeCommand
{
    public RaiseDisputeCommand(Guid TransactionId,
        DisputeReasonCode ReasonCode,
        string Comments)
    {
        this.TransactionId = TransactionId;
        this.ReasonCode = ReasonCode;
        this.Comments = Comments;
    }

    public Guid TransactionId { get; init; }
    public DisputeReasonCode ReasonCode { get; init; }
    public string Comments { get; init; }

    public void Deconstruct(out Guid TransactionId, out DisputeReasonCode ReasonCode, out string Comments)
    {
        TransactionId = this.TransactionId;
        ReasonCode = this.ReasonCode;
        Comments = this.Comments;
    }
}
