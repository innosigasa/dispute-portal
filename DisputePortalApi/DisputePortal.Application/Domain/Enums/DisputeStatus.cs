using DisputePortal.Application.Domain.Extensions;
using DisputePortal.Application.Domain.Models;
using System.ComponentModel;

namespace DisputePortal.Application.Domain.Enums;

public enum DisputeStatus
{
    [Description("Submitted")]
    Submitted = 1,

    [Description("Under Review")]
    UnderReview,

    [Description("Resolved")]
    Resolved,

    [Description("Rejected")]
    Rejected
}

public class DisputeStatusModel : EnumModel
{
}

public static class DisputeStatusExtensions
{
    public static int ToId(this DisputeStatus disputeStatus) => (int)disputeStatus;

    public static DisputeStatus ToDisputeStatus(this int id) => (DisputeStatus)id;

    public static DisputeStatusModel ToModel(this DisputeStatus disputeStatus) =>
        new()
        {
            Id = disputeStatus.ToId(),
            Name = disputeStatus.ToString(),
            Description = disputeStatus.GetDescriptionOrName()
        };
}
