using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Domain.Models;

public class DisputeStatusHistoryModel
{
    public Guid Id { get; set; }
    
    public Guid DisputeId { get; set; }

    public int FromStatusId { get; set; }
    
    public int ToStatusId { get; set; }
    
    public string ChangedByUserId { get; set; } = string.Empty;
    
    public string ChangedByRole { get; set; } = string.Empty;
    
    public string Notes { get; set; } = string.Empty;
    
    public DateTime ChangedAt { get; set; }

    public virtual DisputeModel? Dispute { get; set; } = null!;

    public DisputeStatusModel? FromStatus { get; set; } = null!;
    
    public DisputeStatusModel? ToStatus { get; set; } = null!;
}
