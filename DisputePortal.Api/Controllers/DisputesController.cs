using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.Api.Controllers;

[Route("api/disputes")]
[Authorize(Policy = "CustomerOnly")]
public class DisputesController(IDisputeService service) : BaseController
{
    /// <summary>Raise a new dispute against a transaction.</summary>
    [HttpPost]
    public async Task<IActionResult> RaiseDispute([FromBody] RaiseDisputeCommand command, CancellationToken ct)
        => ToActionResult(await service.RaiseDisputeAsync(command, ct));

    /// <summary>Get a paginated list of the authenticated customer's disputes.</summary>
    [HttpGet]
    public async Task<IActionResult> GetDisputes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DisputeStatus? status = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] DisputeReasonCode? reason = null,
        CancellationToken ct = default)
    {
        var filters = new DisputeFilters(status, dateFrom, dateTo, reason);
        return ToActionResult(await service.GetCustomerDisputesAsync(filters, page, pageSize, ct));
    }

    /// <summary>Get full detail of a single dispute including status history.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDispute(Guid id, CancellationToken ct)
        => ToActionResult(await service.GetDisputeDetailAsync(id, ct));

    /// <summary>Get a summary count of disputes by status for the authenticated customer.</summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
        => ToActionResult(await service.GetSummaryStatsAsync(ct));
}
