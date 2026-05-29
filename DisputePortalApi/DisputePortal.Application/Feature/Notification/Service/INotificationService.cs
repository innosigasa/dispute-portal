using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Notification.Service;

public interface INotificationService
{
    Task SendDisputeNotificationAsync(
        Guid disputeId, Guid customerId, string referenceNumber,
        DisputeStatus status, CancellationToken ct);
}
