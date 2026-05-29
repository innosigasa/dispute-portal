using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Domain.StateMachine;

public static class DisputeStatusTransition
{
    private static readonly Dictionary<DisputeStatus, HashSet<DisputeStatus>> _validTransitions = new()
    {
        [DisputeStatus.Submitted]   = [DisputeStatus.UnderReview],
        [DisputeStatus.UnderReview] = [DisputeStatus.Resolved, DisputeStatus.Rejected],
        [DisputeStatus.Resolved]    = [],
        [DisputeStatus.Rejected]    = []
    };

    public static bool IsValid(DisputeStatus from, DisputeStatus to)
        => _validTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);

    public static IReadOnlySet<DisputeStatus> GetValidNext(DisputeStatus from)
        => _validTransitions.TryGetValue(from, out var allowed)
            ? allowed
            : new HashSet<DisputeStatus>();

    public static void EnsureValid(DisputeStatus from, DisputeStatus to)
    {
        if (!IsValid(from, to))
            throw new InvalidOperationException(
                $"Invalid dispute status transition: {from} → {to}.");
    }
}
