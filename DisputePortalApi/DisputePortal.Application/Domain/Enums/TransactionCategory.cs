using System.ComponentModel;
using DisputePortal.Application.Domain.Extensions;
using DisputePortal.Application.Domain.Models;

namespace DisputePortal.Application.Domain.Enums;

public enum TransactionCategory
{
    [Description("Money Transfer")]
    Transfer = 1,

    [Description("Purchase")]
    Purchase,

    [Description("Cash Withdrawal")]
    Withdrawal,

    [Description("Account Deposit")]
    Deposit,

    [Description("Service Fee")]
    Fee,

    [Description("Bill Payment")]
    BillPayment,

    [Description("Refund")]
    Refund,

    [Description("Salary Payment")]
    Salary,

    [Description("Interest Earned")]
    Interest,

    [Description("Loan Repayment")]
    LoanRepayment,

    [Description("Mobile Recharge")]
    Airtime,

    [Description("Insurance Payment")]
    Insurance,

    [Description("Tax Payment")]
    Tax,

    [Description("ATM Fee")]
    AtmFee,

    [Description("Card Payment")]
    CardPayment,

    [Description("Cheque Deposit")]
    ChequeDeposit,

    [Description("Reversal Transaction")]
    Reversal,

    [Description("Subscription Payment")]
    Subscription,

    [Description("Investment Contribution")]
    Investment,

    [Description("Other Transaction")]
    Other
}

public class TransactionCategoryModel : EnumModel
{
}

public static class TransactionCategoryExtensions
{
    public static int ToId(this TransactionCategory category) => (int)category;

    public static TransactionCategoryModel ToModel(this TransactionCategory category) =>
        new ()
        {
            Id = category.ToId(),
            Name = category.ToString(),
            Description = category.GetDescriptionOrName()
        };
}