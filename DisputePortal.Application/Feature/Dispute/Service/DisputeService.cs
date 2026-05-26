using DisputePortal.Application.Common;
using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Feature.Auth.Service;
using DisputePortal.Application.Feature.Dispute.Persistence;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Results;
using DisputePortal.Application.Feature.Notification.Service;
using DisputePortal.Application.Feature.Transaction.Persistence;
using DisputeEntities = DisputePortal.Application.Domain.Entities;
using FluentValidation;

namespace DisputePortal.Application.Feature.Dispute.Service;

public class DisputeService(
    IDisputeRepository disputeRepo,
    ITransactionRepository txRepo,
    INotificationService notificationService,
    IReferenceNumberGenerator refGen,
    ICurrentUserService currentUser,
    IValidator<RaiseDisputeCommand> raiseValidator) : IDisputeService
{
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
        var dispute = new DisputeEntities.Dispute
        {
            Id = Guid.NewGuid(),
            ReferenceNumber = refGen.Generate(),
            TransactionId = cmd.TransactionId,
            CustomerId = customerId,
            Reason = cmd.ReasonCode,
            Comments = cmd.Comments,
            Status = DisputeStatus.Submitted,
            SubmittedAt = now,
            CreatedAt = now,
            UpdatedAt = now
        };

        dispute.StatusHistory.Add(new DisputeEntities.DisputeStatusHistory
        {
            Id = Guid.NewGuid(),
            DisputeId = dispute.Id,
            FromStatus = DisputeStatus.Submitted,
            ToStatus = DisputeStatus.Submitted,
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
