using DisputePortal.Application.Common;
using DisputePortal.Application.Feature.Auth.Service;
using DisputePortal.Application.Feature.Dispute.Persistence;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Results;
using DisputePortal.Application.Feature.Notification.Service;
using FluentValidation;

namespace DisputePortal.Application.Feature.Dispute.Service;

public class AgentDisputeService(
    IDisputeRepository disputeRepo,
    INotificationService notificationService,
    ICurrentUserService currentUser,
    IValidator<UpdateDisputeStatusCommand> statusValidator) : IAgentDisputeService
{
    public async Task<RequestResult<PagedResult<DisputeListItemDto>>> GetAllDisputesAsync(
        DisputeFilters filters, int page, int pageSize, CancellationToken ct)
    {
        var result = await disputeRepo.GetAllPagedAsync(filters, page, pageSize, ct);
        return RequestResult<PagedResult<DisputeListItemDto>>.Ok(result);
    }

    public async Task<RequestResult<DisputeDetailDto>> GetDisputeDetailAsync(Guid id, CancellationToken ct)
    {
        var result = await disputeRepo.GetByIdAsync(id, ct);
        return result is null
            ? RequestResult<DisputeDetailDto>.NotFound($"Dispute {id} not found.")
            : RequestResult<DisputeDetailDto>.Ok(result);
    }

    public async Task<RequestResult<DisputeDetailDto>> UpdateStatusAsync(UpdateDisputeStatusCommand cmd, CancellationToken ct)
    {
        var validation = await statusValidator.ValidateAsync(cmd, ct);
        if (!validation.IsValid)
            return RequestResult<DisputeDetailDto>.BadRequest(
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        var detail = await disputeRepo.GetByIdAsync(cmd.DisputeId, ct);
        if (detail is null) return RequestResult<DisputeDetailDto>.NotFound($"Dispute {cmd.DisputeId} not found.");

        try
        {
            var agentId = currentUser.UserId;
            var agentRole = currentUser.Role;
            var updated = await disputeRepo.UpdateStatusAsync(cmd.DisputeId, cmd.NewStatus, cmd.Notes, agentId, agentRole, ct);
            if (updated is null) return RequestResult<DisputeDetailDto>.NotFound($"Dispute {cmd.DisputeId} not found.");
            await notificationService.SendDisputeNotificationAsync(cmd.DisputeId, updated.Customer.Id, updated.ReferenceNumber, cmd.NewStatus, ct);
            return RequestResult<DisputeDetailDto>.Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return RequestResult<DisputeDetailDto>.Conflict(ex.Message);
        }
    }
}
