using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Domain.Models;

public class TransactionModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public TransactionCategory Category { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Reference { get; set; } = string.Empty;
    public bool IsDisputed { get; set; }
    public DateTime CreatedAt { get; set; }

    public CustomerModel Customer { get; set; } = null!;
    public BankAccountModel Account { get; set; } = null!;
    public DisputeModel? Dispute { get; set; }
}
