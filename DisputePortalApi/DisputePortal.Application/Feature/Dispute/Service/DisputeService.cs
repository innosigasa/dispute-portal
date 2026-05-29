using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Domain.Models;
using DisputePortal.Application.Feature.Auth.Service;
using DisputePortal.Application.Feature.Dispute.Persistence;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Results;
using DisputePortal.Application.Feature.Notification.Service;
using DisputePortal.Application.Feature.Transaction.Persistence;
using FluentValidation;
using DisputePortal.Application.Domain.Requests;

namespace DisputePortal.Application.Feature.Dispute.Service;

public class DisputeService : IDisputeService
{
    private readonly IDisputeRepository disputeRepo;
    private readonly ITransactionRepository txRepo;
    private readonly INotificationService notificationService;
    private readonly IReferenceNumberGenerator refGen;
    private readonly ICurrentUserService currentUser;
    private readonly IValidator<RaiseDisputeCommand> raiseValidator;

    public DisputeService(IDisputeRepository disputeRepo,
        ITransactionRepository txRepo,
        INotificationService notificationService,
        IReferenceNumberGenerator refGen,
        ICurrentUserService currentUser,
        IValidator<RaiseDisputeCommand> raiseValidator)
    {
        this.disputeRepo = disputeRepo;
        this.txRepo = txRepo;
        this.notificationService = notificationService;
        this.refGen = refGen;
        this.currentUser = currentUser;
        this.raiseValidator = raiseValidator;
    }

    public async Task<RequestResult<DisputeDetailDto>> RaiseDisputeAsync(RaiseDisputeCommand cmd, CancellationToken ct)
    {
        var validation = await raiseValidator.ValidateAsync(cmd, ct);
        if (!validation.IsValid)
            return RequestResult<DisputeDetailDto>.BadRequest(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var customerId = currentUser.CustomerId!.Value;

        var tx = await txRepo.GetByIdAsync(cmd.TransactionId, customerId, ct);
        if (tx is null) return RequestResult<DisputeDetailDto>.NotFound($"Transaction {cmd.TransactionId} not found.");
        if (tx.IsDisputed) return RequestResult<DisputeDetailDto>.Conflict("This transaction already has an active dispute.");

        var now = DateTime.UtcNow;
        var dispute = new DisputeModel
        {
            Id = Guid.NewGuid(),
            ReferenceNumber = refGen.Generate(),
            TransactionId = cmd.TransactionId,
            CustomerId = customerId,
            DisputeReasonId = cmd.ReasonCode.ToId(),
            Comments = cmd.Comments,
            StatusId = DisputeStatus.Submitted.ToId(),
            SubmittedAt = now,
            CreatedAt = now,
            UpdatedAt = now
        };

        dispute.StatusHistory.Add(new DisputeStatusHistoryModel
        {
            Id = Guid.NewGuid(),
            DisputeId = dispute.Id,
            FromStatusId = DisputeStatus.Submitted.ToId(),
            ToStatusId = DisputeStatus.Submitted.ToId(),
            ChangedByUserId = customerId.ToString(),
            ChangedByRole = "customer",
            Notes = "Dispute raised by customer.",
            ChangedAt = now
        });

        await disputeRepo.CreateAsync(dispute, ct);
        await txRepo.SetDisputedAsync(cmd.TransactionId, ct);
        await notificationService.SendDisputeNotificationAsync(dispute.Id, customerId, dispute.ReferenceNumber, DisputeStatus.Submitted, ct);

        var detail = await disputeRepo.GetByIdAsync(dispute.Id, ct);
        return RequestResult<DisputeDetailDto>.Created(detail!);
    }

    public async Task<RequestResult<PagedResult<DisputeListItemDto>>> GetCustomerDisputesAsync(
        DisputeFilters filters, int page, int pageSize, CancellationToken ct)
    {
        var customerId = currentUser.CustomerId!.Value;
        var result = await disputeRepo.GetCustomerDisputesAsync(customerId, filters, page, pageSize, ct);
        return RequestResult<PagedResult<DisputeListItemDto>>.Ok(result);
    }

    public async Task<RequestResult<DisputeDetailDto>> GetDisputeDetailAsync(Guid id, CancellationToken ct)
    {
        var customerId = currentUser.CustomerId!.Value;
        var detail = await disputeRepo.GetByIdAsync(id, ct);
        if (detail is null || detail.Customer.Id != customerId)
            return RequestResult<DisputeDetailDto>.NotFound($"Dispute {id} not found.");
        return RequestResult<DisputeDetailDto>.Ok(detail);
    }

    public async Task<RequestResult<DisputeSummaryStatsDto>> GetSummaryStatsAsync(CancellationToken ct)
    {
        var customerId = currentUser.CustomerId!.Value;
        var stats = await disputeRepo.GetSummaryStatsAsync(customerId, ct);
        return RequestResult<DisputeSummaryStatsDto>.Ok(stats);
    }
}
