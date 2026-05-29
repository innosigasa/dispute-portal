using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Domain.Models;

public class DisputeModel
{
    public Guid Id { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public Guid TransactionId { get; set; }

    public Guid CustomerId { get; set; }

    public int DisputeReasonId { get; set; }

    public string Comments { get; set; } = string.Empty;

    public int StatusId { get; set; }

    public DateTime SubmittedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }

    public DateTime CreatedAt { get; set; }
   
    public DateTime UpdatedAt { get; set; }

    public virtual TransactionModel? Transaction { get; set; }
    
    public virtual CustomerModel? Customer { get; set; }
    
    public virtual DisputeReasonCodeModel? DisputeReason { get; set; }

    public virtual DisputeStatusModel? Status { get; set; }

    public virtual ICollection<DisputeStatusHistoryModel> StatusHistory { get; set; } = [];

    public virtual ICollection<NotificationModel> Notifications { get; set; } = [];
}
