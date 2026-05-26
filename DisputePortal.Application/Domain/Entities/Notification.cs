namespace DisputePortal.Application.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid DisputeId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = null!;
    public Dispute Dispute { get; set; } = null!;
}
