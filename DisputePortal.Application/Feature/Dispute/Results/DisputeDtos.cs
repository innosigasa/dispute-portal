using DisputePortal.Application.Feature.Account.Results;
using DisputePortal.Application.Feature.Transaction.Results;

namespace DisputePortal.Application.Feature.Dispute.Results;

public record DisputeListItemDto(
    Guid Id,
    string ReferenceNumber,
    DateTime TransactionDate,
    decimal Amount,
    string Reason,
    string Status,
    DateTime SubmittedAt,
    DateTime UpdatedAt,
    string? CustomerName,
    string? AccountType,
    string? AccountNumber
);

public record DisputeDetailDto(
    Guid Id,
    string ReferenceNumber,
    string Reason,
    string Comments,
    string Status,
    DateTime SubmittedAt,
    DateTime? ResolvedAt,
    DateTime UpdatedAt,
    TransactionDetailDto Transaction,
    CustomerInfoDto Customer,
    AccountInfoDto Account,
    IReadOnlyList<DisputeStatusHistoryDto> StatusHistory
);

public record DisputeStatusHistoryDto(
    string FromStatus,
    string ToStatus,
    string ChangedByRole,
    string Notes,
    DateTime ChangedAt
);

public record CustomerInfoDto(
    Guid Id,
    string FullName,
    string Email
);

public record DisputeSummaryStatsDto(
    int Submitted,
    int UnderReview,
    int Resolved,
    int Rejected,
    int Total
);
