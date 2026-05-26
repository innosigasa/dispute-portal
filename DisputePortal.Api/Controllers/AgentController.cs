using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.Api.Controllers;

[Route("api/agent/disputes")]
[Authorize(Policy = "AgentOnly")]
public class AgentController(IAgentDisputeService service) : BaseController
{
    /// <summary>Get all disputes across all customers (filterable, paginated).</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllDisputes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DisputeStatus? status = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] DisputeReasonCode? reason = null,
        CancellationToken ct = default)
    {
        var filters = new DisputeFilters(status, dateFrom, dateTo, reason);
        return ToActionResult(await service.GetAllDisputesAsync(filters, page, pageSize, ct));
    }

    /// <summary>Get full dispute detail including customer info and audit trail.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDispute(Guid id, CancellationToken ct)
        => ToActionResult(await service.GetDisputeDetailAsync(id, ct));

    /// <summary>Update the status of a dispute and add resolution notes.</summary>
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateDisputeStatusCommand command, CancellationToken ct)
        => ToActionResult(await service.UpdateStatusAsync(command with { DisputeId = id }, ct));
}
