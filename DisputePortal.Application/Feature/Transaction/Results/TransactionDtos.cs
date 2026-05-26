using DisputePortal.Application.Feature.Account.Results;

namespace DisputePortal.Application.Feature.Transaction.Results;

public record TransactionListItemDto(
    Guid Id,
    DateTime TransactionDate,
    string Description,
    decimal Amount,
    string Category,
    string Reference,
    bool IsDisputed,
    string? DisputeReference,
    Guid AccountId,
    string AccountName,
    string AccountType,
    string AccountNumber
);

public record TransactionDetailDto(
    Guid Id,
    Guid CustomerId,
    DateTime TransactionDate,
    string Description,
    decimal Amount,
    string Category,
    string Reference,
    bool IsDisputed,
    DateTime CreatedAt,
    DisputeSummaryDto? Dispute,
    AccountInfoDto Account
);

public record DisputeSummaryDto(
    Guid Id,
    string ReferenceNumber,
    string Status,
    DateTime SubmittedAt
);
