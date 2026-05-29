using DisputePortal.Application.Feature.Account.Results;
using DisputePortal.Application.Feature.Transaction.Results;

namespace DisputePortal.Application.Feature.Dispute.Results;

public record DisputeListItemDto
{
    public DisputeListItemDto(Guid Id,
        string ReferenceNumber,
        DateTime TransactionDate,
        decimal Amount,
        string Reason,
        string Status,
        DateTime SubmittedAt,
        DateTime UpdatedAt,
        string? CustomerName,
        string? AccountType,
        string? AccountNumber)
    {
        this.Id = Id;
        this.ReferenceNumber = ReferenceNumber;
        this.TransactionDate = TransactionDate;
        this.Amount = Amount;
        this.Reason = Reason;
        this.Status = Status;
        this.SubmittedAt = SubmittedAt;
        this.UpdatedAt = UpdatedAt;
        this.CustomerName = CustomerName;
        this.AccountType = AccountType;
        this.AccountNumber = AccountNumber;
    }

    public Guid Id { get; init; }
    public string ReferenceNumber { get; init; }
    public DateTime TransactionDate { get; init; }
    public decimal Amount { get; init; }
    public string Reason { get; init; }
    public string Status { get; init; }
    public DateTime SubmittedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? CustomerName { get; init; }
    public string? AccountType { get; init; }
    public string? AccountNumber { get; init; }

    public void Deconstruct(out Guid Id, out string ReferenceNumber, out DateTime TransactionDate, out decimal Amount, out string Reason, out string Status, out DateTime SubmittedAt, out DateTime UpdatedAt, out string? CustomerName, out string? AccountType, out string? AccountNumber)
    {
        Id = this.Id;
        ReferenceNumber = this.ReferenceNumber;
        TransactionDate = this.TransactionDate;
        Amount = this.Amount;
        Reason = this.Reason;
        Status = this.Status;
        SubmittedAt = this.SubmittedAt;
        UpdatedAt = this.UpdatedAt;
        CustomerName = this.CustomerName;
        AccountType = this.AccountType;
        AccountNumber = this.AccountNumber;
    }
}

public record DisputeDetailDto
{
    public DisputeDetailDto(Guid Id,
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
        IReadOnlyList<DisputeStatusHistoryDto> StatusHistory)
    {
        this.Id = Id;
        this.ReferenceNumber = ReferenceNumber;
        this.Reason = Reason;
        this.Comments = Comments;
        this.Status = Status;
        this.SubmittedAt = SubmittedAt;
        this.ResolvedAt = ResolvedAt;
        this.UpdatedAt = UpdatedAt;
        this.Transaction = Transaction;
        this.Customer = Customer;
        this.Account = Account;
        this.StatusHistory = StatusHistory;
    }

    public Guid Id { get; init; }
    public string ReferenceNumber { get; init; }
    public string Reason { get; init; }
    public string Comments { get; init; }
    public string Status { get; init; }
    public DateTime SubmittedAt { get; init; }
    public DateTime? ResolvedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public TransactionDetailDto Transaction { get; init; }
    public CustomerInfoDto Customer { get; init; }
    public AccountInfoDto Account { get; init; }
    public IReadOnlyList<DisputeStatusHistoryDto> StatusHistory { get; init; }

    public void Deconstruct(out Guid Id, out string ReferenceNumber, out string Reason, out string Comments, out string Status, out DateTime SubmittedAt, out DateTime? ResolvedAt, out DateTime UpdatedAt, out TransactionDetailDto Transaction, out CustomerInfoDto Customer, out AccountInfoDto Account, out IReadOnlyList<DisputeStatusHistoryDto> StatusHistory)
    {
        Id = this.Id;
        ReferenceNumber = this.ReferenceNumber;
        Reason = this.Reason;
        Comments = this.Comments;
        Status = this.Status;
        SubmittedAt = this.SubmittedAt;
        ResolvedAt = this.ResolvedAt;
        UpdatedAt = this.UpdatedAt;
        Transaction = this.Transaction;
        Customer = this.Customer;
        Account = this.Account;
        StatusHistory = this.StatusHistory;
    }
}

public record DisputeStatusHistoryDto
{
    public DisputeStatusHistoryDto(string FromStatus,
        string ToStatus,
        string ChangedByRole,
        string Notes,
        DateTime ChangedAt)
    {
        this.FromStatus = FromStatus;
        this.ToStatus = ToStatus;
        this.ChangedByRole = ChangedByRole;
        this.Notes = Notes;
        this.ChangedAt = ChangedAt;
    }

    public string FromStatus { get; init; }
    public string ToStatus { get; init; }
    public string ChangedByRole { get; init; }
    public string Notes { get; init; }
    public DateTime ChangedAt { get; init; }

    public void Deconstruct(out string FromStatus, out string ToStatus, out string ChangedByRole, out string Notes, out DateTime ChangedAt)
    {
        FromStatus = this.FromStatus;
        ToStatus = this.ToStatus;
        ChangedByRole = this.ChangedByRole;
        Notes = this.Notes;
        ChangedAt = this.ChangedAt;
    }
}

public record CustomerInfoDto
{
    public CustomerInfoDto(Guid Id,
        string FullName,
        string Email)
    {
        this.Id = Id;
        this.FullName = FullName;
        this.Email = Email;
    }

    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }

    public void Deconstruct(out Guid Id, out string FullName, out string Email)
    {
        Id = this.Id;
        FullName = this.FullName;
        Email = this.Email;
    }
}

public record DisputeSummaryStatsDto
{
    public DisputeSummaryStatsDto(int Submitted,
        int UnderReview,
        int Resolved,
        int Rejected,
        int Total)
    {
        this.Submitted = Submitted;
        this.UnderReview = UnderReview;
        this.Resolved = Resolved;
        this.Rejected = Rejected;
        this.Total = Total;
    }

    public int Submitted { get; init; }
    public int UnderReview { get; init; }
    public int Resolved { get; init; }
    public int Rejected { get; init; }
    public int Total { get; init; }

    public void Deconstruct(out int Submitted, out int UnderReview, out int Resolved, out int Rejected, out int Total)
    {
        Submitted = this.Submitted;
        UnderReview = this.UnderReview;
        Resolved = this.Resolved;
        Rejected = this.Rejected;
        Total = this.Total;
    }
}
