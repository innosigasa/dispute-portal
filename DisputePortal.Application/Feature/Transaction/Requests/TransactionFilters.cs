using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Transaction.Requests;

public record TransactionFilters(
    DateTime? DateFrom,
    DateTime? DateTo,
    TransactionCategory? Category,
    decimal? AmountMin,
    decimal? AmountMax,
    string? Search,
    string SortField = "TransactionDate",
    string SortDirection = "desc"
);
