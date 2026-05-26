using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Domain.Entities;

public class DisputeStatusHistory
{
    public Guid Id { get; set; }
    public Guid DisputeId { get; set; }
    public DisputeStatus FromStatus { get; set; }
    public DisputeStatus ToStatus { get; set; }
    public string ChangedByUserId { get; set; } = string.Empty;
    public string ChangedByRole { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }

    public Dispute Dispute { get; set; } = null!;
}
