namespace DisputePortal.Application.Domain.Models;

public class NotificationModel
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }
    
    public Guid DisputeId { get; set; }
    
    public string Message { get; set; } = string.Empty;
    
    public bool IsRead { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public virtual CustomerModel? Customer { get; set; }
    public virtual DisputeModel? Dispute { get; set; }
}
