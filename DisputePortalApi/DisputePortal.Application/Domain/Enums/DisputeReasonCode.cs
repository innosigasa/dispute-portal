using DisputePortal.Application.Domain.Extensions;
using DisputePortal.Application.Domain.Models;
using System.ComponentModel;

namespace DisputePortal.Application.Domain.Enums;

public enum DisputeReasonCode
{
    [Description("Unauthorised Transaction")]
    UnauthorisedTransaction = 1,

    [Description("Incorrect Transaction Amount")]
    IncorrectAmount,

    [Description("Duplicate Charge")]
    DuplicateCharge,

    [Description("Merchant Dispute")]
    MerchantDispute,

    [Description("Card Not Present Fraud")]
    CardNotPresentFraud,

    [Description("ATM Cash Not Dispensed")]
    AtmCashNotDispensed,

    [Description("ATM Partial Cash Dispensed")]
    AtmPartialCashDispensed,

    [Description("Transaction Reversed but Funds Not Returned")]
    ReversalNotProcessed,

    [Description("Cash Withdrawal Not Authorised")]
    UnauthorizedWithdrawal,

    [Description("Subscription Cancellation Dispute")]
    SubscriptionCancelled,

    [Description("Goods or Services Not Received")]
    GoodsNotReceived,

    [Description("Defective or Damaged Goods")]
    DefectiveGoods,

    [Description("Refund Not Processed")]
    RefundNotProcessed,

    [Description("Incorrect Merchant Charged")]
    IncorrectMerchant,

    [Description("Transaction Processed Multiple Times")]
    MultipleProcessing,

    [Description("Fraudulent Online Transaction")]
    OnlineFraud,

    [Description("Identity Theft Suspected")]
    IdentityTheft,

    [Description("Card Stolen or Lost")]
    LostOrStolenCard,

    [Description("Service Not As Described")]
    ServiceNotAsDescribed,

    [Description("Charge After Cancellation")]
    ChargeAfterCancellation,

    [Description("Late Presentment")]
    LatePresentment,

    [Description("Incorrect Currency Conversion")]
    IncorrectCurrencyConversion,

    [Description("Account Credited Incorrectly")]
    IncorrectCredit,

    [Description("Dispute Already Resolved")]
    AlreadyResolved,

    [Description("Other Dispute Reason")]
    Other
}

public class DisputeReasonCodeModel : EnumModel
{
}

public static class DisputeReasonCodeExtensions
{
    public static int ToId(this DisputeReasonCode disputeReasonCode) => (int)disputeReasonCode;

    public static DisputeReasonCodeModel ToModel(this DisputeReasonCode disputeReasonCode) =>
        new()
        {
            Id = disputeReasonCode.ToId(),
            Name = disputeReasonCode.ToString(),
            Description = disputeReasonCode.GetDescriptionOrName()
        };
}
