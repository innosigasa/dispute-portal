using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Feature.Transaction.Requests;

public record TransactionFilters
{
    public TransactionFilters(DateTime? DateFrom,
        DateTime? DateTo,
        TransactionCategory? Category,
        decimal? AmountMin,
        decimal? AmountMax,
        string? Search,
        string SortField = "TransactionDate",
        string SortDirection = "desc")
    {
        this.DateFrom = DateFrom;
        this.DateTo = DateTo;
        this.Category = Category;
        this.AmountMin = AmountMin;
        this.AmountMax = AmountMax;
        this.Search = Search;
        this.SortField = SortField;
        this.SortDirection = SortDirection;
    }

    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public TransactionCategory? Category { get; init; }
    public decimal? AmountMin { get; init; }
    public decimal? AmountMax { get; init; }
    public string? Search { get; init; }
    public string SortField { get; init; }
    public string SortDirection { get; init; }

    public void Deconstruct(out DateTime? DateFrom, out DateTime? DateTo, out TransactionCategory? Category, out decimal? AmountMin, out decimal? AmountMax, out string? Search, out string SortField, out string SortDirection)
    {
        DateFrom = this.DateFrom;
        DateTo = this.DateTo;
        Category = this.Category;
        AmountMin = this.AmountMin;
        AmountMax = this.AmountMax;
        Search = this.Search;
        SortField = this.SortField;
        SortDirection = this.SortDirection;
    }
}
