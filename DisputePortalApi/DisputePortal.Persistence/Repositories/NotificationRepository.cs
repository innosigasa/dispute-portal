using DisputePortal.Application.Domain.Models;
using DisputePortal.Application.Feature.Notification.Persistence;

namespace DisputePortal.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly DisputePortalDbContext ctx;

    public NotificationRepository(DisputePortalDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task CreateAsync(NotificationModel notification, CancellationToken ct)
    {
        ctx.Notifications.Add(notification);
        await ctx.SaveChangesAsync(ct);
    }
}
