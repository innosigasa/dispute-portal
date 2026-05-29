using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Domain.Models;
using DisputePortal.Application.Feature.Notification.Persistence;
using Microsoft.Extensions.Logging;

namespace DisputePortal.Application.Feature.Notification.Service;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository repo;
    private readonly ILogger<NotificationService> logger;

    public NotificationService(INotificationRepository repo,
        ILogger<NotificationService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task SendDisputeNotificationAsync(
        Guid disputeId, Guid customerId, string referenceNumber,
        DisputeStatus status, CancellationToken ct)
    {
        var message = status switch
        {
            DisputeStatus.Submitted   => $"Your dispute {referenceNumber} has been received.",
            DisputeStatus.UnderReview => $"Your dispute {referenceNumber} is under review by our team.",
            DisputeStatus.Resolved    => $"Your dispute {referenceNumber} has been resolved.",
            DisputeStatus.Rejected    => $"Your dispute {referenceNumber} could not be upheld.",
            _                         => $"Your dispute {referenceNumber} status has been updated."
        };

        var notification = new NotificationModel
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            DisputeId = disputeId,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await repo.CreateAsync(notification, ct);
        logger.LogInformation("[NOTIFICATION] CustomerId={CustomerId} DisputeId={DisputeId} Message={Message}",
            customerId, disputeId, message);
    }
}
