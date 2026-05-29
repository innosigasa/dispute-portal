using DisputePortal.Application.Domain.Extensions;
using DisputePortal.Application.Domain.Models;
using System.ComponentModel;

namespace DisputePortal.Application.Domain.Enums;

public enum AccountType
{
    [Description("Savings Account")]
    Savings = 1,

    [Description("Cheque Account")]
    Cheque,

    [Description("Current Account")]
    Current,

    [Description("Credit Account")]
    Credit,

    [Description("Business Account")]
    Business,

    [Description("Fixed Deposit Account")]
    FixedDeposit,

    [Description("Investment Account")]
    Investment,

    [Description("Loan Account")]
    Loan,

    [Description("Joint Account")]
    Joint,

    [Description("Foreign Currency Account")]
    ForeignCurrency,

    [Description("Student Account")]
    Student,

    [Description("Retirement Account")]
    Retirement,

    [Description("Money Market Account")]
    MoneyMarket,

    [Description("Islamic Banking Account")]
    Islamic,

    [Description("Trust Account")]
    Trust,

    [Description("Digital Wallet Account")]
    DigitalWallet,

    [Description("Corporate Account")]
    Corporate,

    [Description("Offshore Account")]
    Offshore
}

public class AccountTypeModel : EnumModel
{
}

public static class AccountTypeExtensions
{
    public static int ToId(this AccountType accountType) => (int)accountType;

    public static AccountTypeModel ToModel(this AccountType accountType) =>
        new()
        {
            Id = accountType.ToId(),
            Name = accountType.ToString(),
            Description = accountType.GetDescriptionOrName()
        };
}
