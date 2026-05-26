using DisputePortal.Application.Domain.Entities;
using DisputePortal.Application.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DisputePortal.Persistence.Seed;

public static class DataSeeder
{
    private static readonly Guid AliceId = Guid.Parse("11111111-0000-0000-0000-000000000001");
    private static readonly Guid BobId   = Guid.Parse("22222222-0000-0000-0000-000000000002");

    // Alice's accounts
    private static readonly Guid AliceSavingsId = Guid.Parse("aaaaaaaa-0001-0000-0000-000000000001");
    private static readonly Guid AliceChequeId  = Guid.Parse("aaaaaaaa-0001-0000-0000-000000000002");
    private static readonly Guid AliceCreditId  = Guid.Parse("aaaaaaaa-0001-0000-0000-000000000003");

    // Bob's accounts
    private static readonly Guid BobCurrentId = Guid.Parse("bbbbbbbb-0002-0000-0000-000000000001");
    private static readonly Guid BobSavingsId  = Guid.Parse("bbbbbbbb-0002-0000-0000-000000000002");

    public static async Task SeedAsync(DisputePortalDbContext ctx)
    {
        await SeedLookupsAsync(ctx);
        await SeedUsersAsync(ctx);
        await SeedBankAccountsAsync(ctx);
        await SeedTransactionsAsync(ctx);
        await SeedDisputesAsync(ctx);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedLookupsAsync(DisputePortalDbContext ctx)
    {
        if (await ctx.DisputeReasons.AnyAsync()) return;

        ctx.DisputeReasons.AddRange(
            new DisputeReason { Id = 1, Code = "UnauthorisedTransaction", Label = "Unauthorised Transaction" },
            new DisputeReason { Id = 2, Code = "IncorrectAmount",         Label = "Incorrect Amount" },
            new DisputeReason { Id = 3, Code = "DuplicateCharge",         Label = "Duplicate Charge" },
            new DisputeReason { Id = 4, Code = "MerchantDispute",         Label = "Merchant Dispute" },
            new DisputeReason { Id = 5, Code = "Other",                   Label = "Other" }
        );

        ctx.TransactionCategories.AddRange(
            new TransactionCategoryLookup { Id = 1, Code = "Transfer",    Label = "Transfer" },
            new TransactionCategoryLookup { Id = 2, Code = "Purchase",    Label = "Purchase" },
            new TransactionCategoryLookup { Id = 3, Code = "Withdrawal",  Label = "Withdrawal" },
            new TransactionCategoryLookup { Id = 4, Code = "Deposit",     Label = "Deposit" },
            new TransactionCategoryLookup { Id = 5, Code = "Fee",         Label = "Fee" }
        );
    }

    private static async Task SeedUsersAsync(DisputePortalDbContext ctx)
    {
        if (await ctx.Users.AnyAsync()) return;

        ctx.Customers.AddRange(
            new Customer { Id = AliceId, FullName = "Alice Johnson", Email = "alice@example.com", IdentityNumber = "8501015009087", CreatedAt = DateTime.UtcNow },
            new Customer { Id = BobId,   FullName = "Bob Smith",     Email = "bob@example.com",   IdentityNumber = "7908125012083", CreatedAt = DateTime.UtcNow }
        );

        ctx.Users.AddRange(
            new AppUser { Id = Guid.NewGuid(), Email = "alice@example.com", PasswordHash = HashPassword("Password1!"), Role = "customer", CustomerId = AliceId, CreatedAt = DateTime.UtcNow },
            new AppUser { Id = Guid.NewGuid(), Email = "bob@example.com",   PasswordHash = HashPassword("Password1!"), Role = "customer", CustomerId = BobId,   CreatedAt = DateTime.UtcNow },
            new AppUser { Id = Guid.NewGuid(), Email = "agent@bank.com",    PasswordHash = HashPassword("Password1!"), Role = "agent",    CustomerId = null,    CreatedAt = DateTime.UtcNow }
        );
    }

    private static async Task SeedBankAccountsAsync(DisputePortalDbContext ctx)
    {
        if (await ctx.BankAccounts.AnyAsync()) return;

        ctx.BankAccounts.AddRange(
            // Alice — 3 accounts
            new BankAccount { Id = AliceSavingsId, CustomerId = AliceId, AccountNumber = "4001-2345-6789", AccountType = AccountType.Savings, AccountName = "Alice's Savings",  Balance = 12_450.00m, Currency = "ZAR", IsDefault = true,  CreatedAt = DateTime.UtcNow },
            new BankAccount { Id = AliceChequeId,  CustomerId = AliceId, AccountNumber = "4001-9876-5432", AccountType = AccountType.Cheque,  AccountName = "Alice's Cheque",   Balance =  3_200.50m, Currency = "ZAR", IsDefault = false, CreatedAt = DateTime.UtcNow },
            new BankAccount { Id = AliceCreditId,  CustomerId = AliceId, AccountNumber = "4001-1111-2222", AccountType = AccountType.Credit,  AccountName = "Alice's Credit",   Balance = -1_540.00m, Currency = "ZAR", IsDefault = false, CreatedAt = DateTime.UtcNow },

            // Bob — 2 accounts
            new BankAccount { Id = BobCurrentId, CustomerId = BobId, AccountNumber = "4009-8765-4321", AccountType = AccountType.Current, AccountName = "Bob's Current",  Balance =  8_775.00m, Currency = "ZAR", IsDefault = true,  CreatedAt = DateTime.UtcNow },
            new BankAccount { Id = BobSavingsId,  CustomerId = BobId, AccountNumber = "4009-1234-5678", AccountType = AccountType.Savings, AccountName = "Bob's Savings",   Balance = 25_000.00m, Currency = "ZAR", IsDefault = false, CreatedAt = DateTime.UtcNow }
        );
    }

    private static async Task SeedTransactionsAsync(DisputePortalDbContext ctx)
    {
        if (await ctx.Transactions.AnyAsync()) return;

        var aliceAccounts = new[] { AliceSavingsId, AliceSavingsId, AliceSavingsId, AliceChequeId, AliceChequeId, AliceCreditId };
        var bobAccounts   = new[] { BobCurrentId, BobCurrentId, BobCurrentId, BobCurrentId, BobSavingsId, BobSavingsId };

        var categories = Enum.GetValues<TransactionCategory>();
        var descriptions = new[]
        {
            "Grocery Store", "Netflix Subscription", "Petrol Station", "Online Transfer",
            "ATM Withdrawal", "Restaurant Bill", "Electricity Bill", "Amazon Purchase",
            "Gym Membership", "Pharmacy", "Coffee Shop", "Uber Ride", "Clothing Store",
            "Hardware Store", "Medical Aid Premium", "Insurance Premium", "Bank Fee",
            "Salary Deposit", "Freelance Payment", "Book Store"
        };

        var rng      = new Random(42);
        var baseDate = DateTime.UtcNow.AddDays(-180);

        foreach (var (customerId, accountPool) in new[] { (AliceId, aliceAccounts), (BobId, bobAccounts) })
        {
            for (var i = 0; i < 50; i++)
            {
                ctx.Transactions.Add(new Transaction
                {
                    Id              = Guid.NewGuid(),
                    CustomerId      = customerId,
                    AccountId       = accountPool[rng.Next(accountPool.Length)],
                    Amount          = Math.Round((decimal)(rng.NextDouble() * 4950 + 50), 2),
                    Description     = descriptions[rng.Next(descriptions.Length)],
                    Category        = categories[rng.Next(categories.Length)],
                    TransactionDate = baseDate.AddDays(rng.Next(180)),
                    Reference       = $"TXN{rng.Next(100000, 999999)}",
                    IsDisputed      = false,
                    CreatedAt       = DateTime.UtcNow
                });
            }
        }
    }

    private static async Task SeedDisputesAsync(DisputePortalDbContext ctx)
    {
        if (await ctx.Disputes.AnyAsync()) return;

        await ctx.SaveChangesAsync();

        var aliceTxns = await ctx.Transactions
            .Where(t => t.CustomerId == AliceId)
            .Take(4)
            .ToListAsync();

        var bobTxns = await ctx.Transactions
            .Where(t => t.CustomerId == BobId)
            .Take(3)
            .ToListAsync();

        var disputes = new List<(Transaction tx, DisputeStatus status, DisputeReasonCode reason, string comments)>
        {
            (aliceTxns[0], DisputeStatus.Resolved,    DisputeReasonCode.UnauthorisedTransaction, "I did not authorise this transaction."),
            (aliceTxns[1], DisputeStatus.Rejected,    DisputeReasonCode.IncorrectAmount,         "The amount charged is incorrect."),
            (aliceTxns[2], DisputeStatus.UnderReview, DisputeReasonCode.DuplicateCharge,         "This charge appears twice on my statement."),
            (aliceTxns[3], DisputeStatus.Submitted,   DisputeReasonCode.MerchantDispute,         "Merchant did not deliver goods."),
            (bobTxns[0],   DisputeStatus.Resolved,    DisputeReasonCode.Other,                   "Unexplained charge on account."),
            (bobTxns[1],   DisputeStatus.UnderReview, DisputeReasonCode.UnauthorisedTransaction, "Card was used without my knowledge."),
            (bobTxns[2],   DisputeStatus.Submitted,   DisputeReasonCode.IncorrectAmount,         "Charged double the agreed price."),
        };

        var seq = 100;
        foreach (var (tx, status, reason, comments) in disputes)
        {
            var disputeId   = Guid.NewGuid();
            var submittedAt = DateTime.UtcNow.AddDays(-30);

            ctx.Disputes.Add(new Dispute
            {
                Id              = disputeId,
                ReferenceNumber = $"DSP-2026-{seq++:D6}",
                TransactionId   = tx.Id,
                CustomerId      = tx.CustomerId,
                Reason          = reason,
                Comments        = comments,
                Status          = status,
                SubmittedAt     = submittedAt,
                ResolvedAt      = status is DisputeStatus.Resolved or DisputeStatus.Rejected ? submittedAt.AddDays(5) : null,
                CreatedAt       = submittedAt,
                UpdatedAt       = DateTime.UtcNow
            });

            tx.IsDisputed = true;

            AddHistory(ctx, disputeId, DisputeStatus.Submitted,  DisputeStatus.Submitted,  submittedAt,             "Dispute raised by customer.",                           "system",        "system");

            if (status >= DisputeStatus.UnderReview)
                AddHistory(ctx, disputeId, DisputeStatus.Submitted,  DisputeStatus.UnderReview, submittedAt.AddDays(1), "Agent picked up for review.",                        "agent-seed-id", "agent");

            if (status == DisputeStatus.Resolved)
                AddHistory(ctx, disputeId, DisputeStatus.UnderReview, DisputeStatus.Resolved,   submittedAt.AddDays(5), "Transaction confirmed as fraudulent. Full refund issued.", "agent-seed-id", "agent");

            if (status == DisputeStatus.Rejected)
                AddHistory(ctx, disputeId, DisputeStatus.UnderReview, DisputeStatus.Rejected,   submittedAt.AddDays(5), "Insufficient evidence to support the claim.",         "agent-seed-id", "agent");
        }
    }

    private static void AddHistory(
        DisputePortalDbContext ctx, Guid disputeId,
        DisputeStatus from, DisputeStatus to, DateTime at, string notes, string userId, string role)
    {
        ctx.DisputeStatusHistories.Add(new DisputeStatusHistory
        {
            Id              = Guid.NewGuid(),
            DisputeId       = disputeId,
            FromStatus      = from,
            ToStatus        = to,
            ChangedByUserId = userId,
            ChangedByRole   = role,
            Notes           = notes,
            ChangedAt       = at
        });
    }

    private static string HashPassword(string password)
    {
        using var sha  = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password + "dispute-portal-salt"));
        return Convert.ToBase64String(bytes);
    }

    public static bool VerifyPassword(string password, string hash)
        => HashPassword(password) == hash;
}
