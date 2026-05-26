namespace DisputePortal.Application.Feature.Account.Results;

public record BankAccountDto(
    Guid Id,
    string AccountNumber,
    string AccountType,
    string AccountName,
    decimal Balance,
    string Currency,
    bool IsDefault
);

public record AccountInfoDto(
    Guid Id,
    string AccountName,
    string AccountType,
    string AccountNumber
);
