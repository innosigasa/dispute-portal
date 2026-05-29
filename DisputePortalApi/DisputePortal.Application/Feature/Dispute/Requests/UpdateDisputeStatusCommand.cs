using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Dispute.Requests;

public record UpdateDisputeStatusCommand
{
    public UpdateDisputeStatusCommand(Guid DisputeId,
        DisputeStatus NewStatus,
        string Notes)
    {
        this.DisputeId = DisputeId;
        this.NewStatus = NewStatus;
        this.Notes = Notes;
    }

    public Guid DisputeId { get; init; }
    public DisputeStatus NewStatus { get; init; }
    public string Notes { get; init; }

    public void Deconstruct(out Guid DisputeId, out DisputeStatus NewStatus, out string Notes)
    {
        DisputeId = this.DisputeId;
        NewStatus = this.NewStatus;
        Notes = this.Notes;
    }
}
