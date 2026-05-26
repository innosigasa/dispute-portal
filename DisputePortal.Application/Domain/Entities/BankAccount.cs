using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Domain.Entities;

public class BankAccount
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "ZAR";
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
