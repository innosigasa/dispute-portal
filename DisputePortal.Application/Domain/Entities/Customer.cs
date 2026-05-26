namespace DisputePortal.Application.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ICollection<BankAccount> Accounts { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<Dispute> Disputes { get; set; } = [];
}
