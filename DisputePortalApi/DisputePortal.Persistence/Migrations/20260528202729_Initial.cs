using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DisputePortal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Data");

            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "Static");

            migrationBuilder.CreateTable(
                name: "AccountType",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(256)", nullable: false),
                    Description = table.Column<string>(type: "varchar(512)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "varchar(256)", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "varchar(256)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "varchar(256)", nullable: false),
                    Email = table.Column<string>(type: "varchar(256)", nullable: false),
                    IdentityNumber = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisputeReasonCode",
                schema: "Static",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(256)", nullable: false),
                    Description = table.Column<string>(type: "varchar(512)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisputeReasonCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisputeStatus",
                schema: "Static",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "varchar(256)", nullable: false),
                    Description = table.Column<string>(type: "varchar(512)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisputeStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCategory",
                schema: "Static",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AppUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNumber = table.Column<string>(type: "varchar(50)", nullable: false),
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false),
                    AccountName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "char(3)", nullable: false, defaultValue: "ZAR"),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccount_AccountType_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalSchema: "Data",
                        principalTable: "AccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccount_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Data",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reference = table.Column<string>(type: "varchar(100)", nullable: false),
                    IsDisputed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_BankAccount_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Data",
                        principalTable: "BankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Data",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dispute",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "varchar(256)", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisputeReasonId = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispute_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Data",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dispute_DisputeReasonCode_DisputeReasonId",
                        column: x => x.DisputeReasonId,
                        principalSchema: "Static",
                        principalTable: "DisputeReasonCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dispute_DisputeStatus_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "Static",
                        principalTable: "DisputeStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dispute_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "Data",
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisputeStatusHistory",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisputeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatusId = table.Column<int>(type: "integer", nullable: false),
                    ToStatusId = table.Column<int>(type: "integer", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "varchar(256)", nullable: false),
                    ChangedByRole = table.Column<string>(type: "varchar(256)", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisputeStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisputeStatusHistory_DisputeStatus_FromStatusId",
                        column: x => x.FromStatusId,
                        principalSchema: "Static",
                        principalTable: "DisputeStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisputeStatusHistory_DisputeStatus_ToStatusId",
                        column: x => x.ToStatusId,
                        principalSchema: "Static",
                        principalTable: "DisputeStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisputeStatusHistory_Dispute_DisputeId",
                        column: x => x.DisputeId,
                        principalSchema: "Data",
                        principalTable: "Dispute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisputeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Data",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notification_Dispute_DisputeId",
                        column: x => x.DisputeId,
                        principalSchema: "Data",
                        principalTable: "Dispute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Data",
                table: "AccountType",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Savings Account", "Savings" },
                    { 2, "Cheque Account", "Cheque" },
                    { 3, "Current Account", "Current" },
                    { 4, "Credit Account", "Credit" },
                    { 5, "Business Account", "Business" },
                    { 6, "Fixed Deposit Account", "FixedDeposit" },
                    { 7, "Investment Account", "Investment" },
                    { 8, "Loan Account", "Loan" },
                    { 9, "Joint Account", "Joint" },
                    { 10, "Foreign Currency Account", "ForeignCurrency" },
                    { 11, "Student Account", "Student" },
                    { 12, "Retirement Account", "Retirement" },
                    { 13, "Money Market Account", "MoneyMarket" },
                    { 14, "Islamic Banking Account", "Islamic" },
                    { 15, "Trust Account", "Trust" },
                    { 16, "Digital Wallet Account", "DigitalWallet" },
                    { 17, "Corporate Account", "Corporate" },
                    { 18, "Offshore Account", "Offshore" }
                });

            migrationBuilder.InsertData(
                schema: "Static",
                table: "DisputeReasonCode",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Unauthorised Transaction", "UnauthorisedTransaction" },
                    { 2, "Incorrect Transaction Amount", "IncorrectAmount" },
                    { 3, "Duplicate Charge", "DuplicateCharge" },
                    { 4, "Merchant Dispute", "MerchantDispute" },
                    { 5, "Card Not Present Fraud", "CardNotPresentFraud" },
                    { 6, "ATM Cash Not Dispensed", "AtmCashNotDispensed" },
                    { 7, "ATM Partial Cash Dispensed", "AtmPartialCashDispensed" },
                    { 8, "Transaction Reversed but Funds Not Returned", "ReversalNotProcessed" },
                    { 9, "Cash Withdrawal Not Authorised", "UnauthorizedWithdrawal" },
                    { 10, "Subscription Cancellation Dispute", "SubscriptionCancelled" },
                    { 11, "Goods or Services Not Received", "GoodsNotReceived" },
                    { 12, "Defective or Damaged Goods", "DefectiveGoods" },
                    { 13, "Refund Not Processed", "RefundNotProcessed" },
                    { 14, "Incorrect Merchant Charged", "IncorrectMerchant" },
                    { 15, "Transaction Processed Multiple Times", "MultipleProcessing" },
                    { 16, "Fraudulent Online Transaction", "OnlineFraud" },
                    { 17, "Identity Theft Suspected", "IdentityTheft" },
                    { 18, "Card Stolen or Lost", "LostOrStolenCard" },
                    { 19, "Service Not As Described", "ServiceNotAsDescribed" },
                    { 20, "Charge After Cancellation", "ChargeAfterCancellation" },
                    { 21, "Late Presentment", "LatePresentment" },
                    { 22, "Incorrect Currency Conversion", "IncorrectCurrencyConversion" },
                    { 23, "Account Credited Incorrectly", "IncorrectCredit" },
                    { 24, "Dispute Already Resolved", "AlreadyResolved" },
                    { 25, "Other Dispute Reason", "Other" }
                });

            migrationBuilder.InsertData(
                schema: "Static",
                table: "DisputeStatus",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Submitted", "Submitted" },
                    { 2, "Under Review", "UnderReview" },
                    { 3, "Resolved", "Resolved" },
                    { 4, "Rejected", "Rejected" }
                });

            migrationBuilder.InsertData(
                schema: "Static",
                table: "TransactionCategory",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Money Transfer", "Transfer" },
                    { 2, "Purchase", "Purchase" },
                    { 3, "Cash Withdrawal", "Withdrawal" },
                    { 4, "Account Deposit", "Deposit" },
                    { 5, "Service Fee", "Fee" },
                    { 6, "Bill Payment", "BillPayment" },
                    { 7, "Refund", "Refund" },
                    { 8, "Salary Payment", "Salary" },
                    { 9, "Interest Earned", "Interest" },
                    { 10, "Loan Repayment", "LoanRepayment" },
                    { 11, "Mobile Recharge", "Airtime" },
                    { 12, "Insurance Payment", "Insurance" },
                    { 13, "Tax Payment", "Tax" },
                    { 14, "ATM Fee", "AtmFee" },
                    { 15, "Card Payment", "CardPayment" },
                    { 16, "Cheque Deposit", "ChequeDeposit" },
                    { 17, "Reversal Transaction", "Reversal" },
                    { 18, "Subscription Payment", "Subscription" },
                    { 19, "Investment Contribution", "Investment" },
                    { 20, "Other Transaction", "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountType_Name",
                schema: "Data",
                table: "AccountType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_Email",
                schema: "Security",
                table: "AppUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_AccountNumber",
                schema: "Data",
                table: "BankAccount",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_AccountTypeId",
                schema: "Data",
                table: "BankAccount",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_CustomerId",
                schema: "Data",
                table: "BankAccount",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email",
                schema: "Data",
                table: "Customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_CustomerId_StatusId",
                schema: "Data",
                table: "Dispute",
                columns: new[] { "CustomerId", "StatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_DisputeReasonId",
                schema: "Data",
                table: "Dispute",
                column: "DisputeReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_ReferenceNumber",
                schema: "Data",
                table: "Dispute",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_StatusId",
                schema: "Data",
                table: "Dispute",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_TransactionId",
                schema: "Data",
                table: "Dispute",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisputeReasonCode_Name",
                schema: "Static",
                table: "DisputeReasonCode",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisputeStatus_Name",
                schema: "Static",
                table: "DisputeStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisputeStatusHistory_DisputeId_ChangedAt",
                schema: "Data",
                table: "DisputeStatusHistory",
                columns: new[] { "DisputeId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_DisputeStatusHistory_FromStatusId",
                schema: "Data",
                table: "DisputeStatusHistory",
                column: "FromStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DisputeStatusHistory_ToStatusId",
                schema: "Data",
                table: "DisputeStatusHistory",
                column: "ToStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_CustomerId",
                schema: "Data",
                table: "Notification",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_DisputeId",
                schema: "Data",
                table: "Notification",
                column: "DisputeId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId_ExpiresAt",
                schema: "Security",
                table: "RefreshToken",
                columns: new[] { "UserId", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                schema: "Data",
                table: "Transaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CustomerId_TransactionDate",
                schema: "Data",
                table: "Transaction",
                columns: new[] { "CustomerId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_Name",
                schema: "Static",
                table: "TransactionCategory",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisputeStatusHistory",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "TransactionCategory",
                schema: "Static");

            migrationBuilder.DropTable(
                name: "Dispute",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "AppUser",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "DisputeReasonCode",
                schema: "Static");

            migrationBuilder.DropTable(
                name: "DisputeStatus",
                schema: "Static");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "AccountType",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "Data");
        }
    }
}
