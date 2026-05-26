using DisputePortal.Application.Domain.Entities;
using DisputePortal.Application.Feature.Notification.Persistence;

namespace DisputePortal.Persistence.Repositories;

public class NotificationRepository(DisputePortalDbContext ctx) : INotificationRepository
{
    public async Task CreateAsync(Notification notification, CancellationToken ct)
    {
        ctx.Notifications.Add(notification);
        await ctx.SaveChangesAsync(ct);
    }
}
