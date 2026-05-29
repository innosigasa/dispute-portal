using DisputePortal.Application.Domain.Enums;

namespace DisputePortal.Application.Domain.Models;

public class BankAccountModel
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }
    
    public string AccountNumber { get; set; } = string.Empty;
    
    public int AccountTypeId { get; set; }
    
    public string AccountName { get; set; } = string.Empty;
    
    public decimal Balance { get; set; }
    
    public string Currency { get; set; } = AppConstants.CurrencyZAR;
    
    public bool IsDefault { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public virtual CustomerModel? Customer { get; set; }

    public virtual AccountTypeModel? AccountType { get; set; }

    public virtual ICollection<TransactionModel> Transactions { get; set; } = [];
}
