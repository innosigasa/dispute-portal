using DisputePortal.Application.Feature.Account.Results;

namespace DisputePortal.Application.Feature.Transaction.Results;

public record TransactionListItemDto
{
    public TransactionListItemDto(Guid Id,
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
        string AccountNumber)
    {
        this.Id = Id;
        this.TransactionDate = TransactionDate;
        this.Description = Description;
        this.Amount = Amount;
        this.Category = Category;
        this.Reference = Reference;
        this.IsDisputed = IsDisputed;
        this.DisputeReference = DisputeReference;
        this.AccountId = AccountId;
        this.AccountName = AccountName;
        this.AccountType = AccountType;
        this.AccountNumber = AccountNumber;
    }

    public Guid Id { get; init; }
    public DateTime TransactionDate { get; init; }
    public string Description { get; init; }
    public decimal Amount { get; init; }
    public string Category { get; init; }
    public string Reference { get; init; }
    public bool IsDisputed { get; init; }
    public string? DisputeReference { get; init; }
    public Guid AccountId { get; init; }
    public string AccountName { get; init; }
    public string AccountType { get; init; }
    public string AccountNumber { get; init; }

    public void Deconstruct(out Guid Id, out DateTime TransactionDate, out string Description, out decimal Amount, out string Category, out string Reference, out bool IsDisputed, out string? DisputeReference, out Guid AccountId, out string AccountName, out string AccountType, out string AccountNumber)
    {
        Id = this.Id;
        TransactionDate = this.TransactionDate;
        Description = this.Description;
        Amount = this.Amount;
        Category = this.Category;
        Reference = this.Reference;
        IsDisputed = this.IsDisputed;
        DisputeReference = this.DisputeReference;
        AccountId = this.AccountId;
        AccountName = this.AccountName;
        AccountType = this.AccountType;
        AccountNumber = this.AccountNumber;
    }
}

public record TransactionDetailDto
{
    public TransactionDetailDto(Guid Id,
        Guid CustomerId,
        DateTime TransactionDate,
        string Description,
        decimal Amount,
        string Category,
        string Reference,
        bool IsDisputed,
        DateTime CreatedAt,
        DisputeSummaryDto? Dispute,
        AccountInfoDto Account)
    {
        this.Id = Id;
        this.CustomerId = CustomerId;
        this.TransactionDate = TransactionDate;
        this.Description = Description;
        this.Amount = Amount;
        this.Category = Category;
        this.Reference = Reference;
        this.IsDisputed = IsDisputed;
        this.CreatedAt = CreatedAt;
        this.Dispute = Dispute;
        this.Account = Account;
    }

    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public DateTime TransactionDate { get; init; }
    public string Description { get; init; }
    public decimal Amount { get; init; }
    public string Category { get; init; }
    public string Reference { get; init; }
    public bool IsDisputed { get; init; }
    public DateTime CreatedAt { get; init; }
    public DisputeSummaryDto? Dispute { get; init; }
    public AccountInfoDto Account { get; init; }

    public void Deconstruct(out Guid Id, out Guid CustomerId, out DateTime TransactionDate, out string Description, out decimal Amount, out string Category, out string Reference, out bool IsDisputed, out DateTime CreatedAt, out DisputeSummaryDto? Dispute, out AccountInfoDto Account)
    {
        Id = this.Id;
        CustomerId = this.CustomerId;
        TransactionDate = this.TransactionDate;
        Description = this.Description;
        Amount = this.Amount;
        Category = this.Category;
        Reference = this.Reference;
        IsDisputed = this.IsDisputed;
        CreatedAt = this.CreatedAt;
        Dispute = this.Dispute;
        Account = this.Account;
    }
}

public record DisputeSummaryDto
{
    public DisputeSummaryDto(Guid Id,
        string ReferenceNumber,
        string Status,
        DateTime SubmittedAt)
    {
        this.Id = Id;
        this.ReferenceNumber = ReferenceNumber;
        this.Status = Status;
        this.SubmittedAt = SubmittedAt;
    }

    public Guid Id { get; init; }
    public string ReferenceNumber { get; init; }
    public string Status { get; init; }
    public DateTime SubmittedAt { get; init; }

    public void Deconstruct(out Guid Id, out string ReferenceNumber, out string Status, out DateTime SubmittedAt)
    {
        Id = this.Id;
        ReferenceNumber = this.ReferenceNumber;
        Status = this.Status;
        SubmittedAt = this.SubmittedAt;
    }
}
