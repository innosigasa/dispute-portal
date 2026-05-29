namespace DisputePortal.Application.Domain.Models;

public class CustomerModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string IdentityNumber { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<BankAccountModel> Accounts { get; set; } = [];
    
    public virtual ICollection<TransactionModel> Transactions { get; set; } = [];
    
    public virtual ICollection<DisputeModel> Disputes { get; set; } = [];
}
