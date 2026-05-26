namespace DisputePortal.Application.Feature.Notification.Persistence;

public interface INotificationRepository
{
    Task CreateAsync(global::DisputePortal.Application.Domain.Entities.Notification notification, CancellationToken ct);
}
