using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Domain.Entities;

public class Dispute
{
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public Guid TransactionId { get; set; }
    public Guid CustomerId { get; set; }
    public DisputeReasonCode Reason { get; set; }
    public string Comments { get; set; } = string.Empty;
    public DisputeStatus Status { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Transaction Transaction { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public ICollection<DisputeStatusHistory> StatusHistory { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}
