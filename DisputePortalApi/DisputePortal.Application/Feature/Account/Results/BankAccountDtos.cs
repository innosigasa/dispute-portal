namespace DisputePortal.Application.Feature.Account.Results;

public record BankAccountDto
{
    public BankAccountDto(Guid Id,
        string AccountNumber,
        string AccountType,
        string AccountName,
        decimal Balance,
        string Currency,
        bool IsDefault)
    {
        this.Id = Id;
        this.AccountNumber = AccountNumber;
        this.AccountType = AccountType;
        this.AccountName = AccountName;
        this.Balance = Balance;
        this.Currency = Currency;
        this.IsDefault = IsDefault;
    }

    public Guid Id { get; init; }
    public string AccountNumber { get; init; }
    public string AccountType { get; init; }
    public string AccountName { get; init; }
    public decimal Balance { get; init; }
    public string Currency { get; init; }
    public bool IsDefault { get; init; }

    public void Deconstruct(out Guid Id, out string AccountNumber, out string AccountType, out string AccountName, out decimal Balance, out string Currency, out bool IsDefault)
    {
        Id = this.Id;
        AccountNumber = this.AccountNumber;
        AccountType = this.AccountType;
        AccountName = this.AccountName;
        Balance = this.Balance;
        Currency = this.Currency;
        IsDefault = this.IsDefault;
    }
}

public record AccountInfoDto
{
    public AccountInfoDto(Guid Id,
        string AccountName,
        string AccountType,
        string AccountNumber)
    {
        this.Id = Id;
        this.AccountName = AccountName;
        this.AccountType = AccountType;
        this.AccountNumber = AccountNumber;
    }

    public Guid Id { get; init; }
    public string AccountName { get; init; }
    public string AccountType { get; init; }
    public string AccountNumber { get; init; }

    public void Deconstruct(out Guid Id, out string AccountName, out string AccountType, out string AccountNumber)
    {
        Id = this.Id;
        AccountName = this.AccountName;
        AccountType = this.AccountType;
        AccountNumber = this.AccountNumber;
    }
}
