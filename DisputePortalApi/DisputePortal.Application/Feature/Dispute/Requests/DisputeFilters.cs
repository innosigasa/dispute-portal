using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Dispute.Requests;

public record DisputeFilters
{
    public DisputeFilters(DisputeStatus? Status,
        DateTime? DateFrom,
        DateTime? DateTo,
        DisputeReasonCode? ReasonCode)
    {
        this.Status = Status;
        this.DateFrom = DateFrom;
        this.DateTo = DateTo;
        this.ReasonCode = ReasonCode;
    }

    public DisputeStatus? Status { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public DisputeReasonCode? ReasonCode { get; init; }

    public void Deconstruct(out DisputeStatus? Status, out DateTime? DateFrom, out DateTime? DateTo, out DisputeReasonCode? ReasonCode)
    {
        Status = this.Status;
        DateFrom = this.DateFrom;
        DateTo = this.DateTo;
        ReasonCode = this.ReasonCode;
    }
}
