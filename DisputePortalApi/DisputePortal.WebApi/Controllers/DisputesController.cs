using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Feature.Dispute.Requests;
using DisputePortal.Application.Feature.Dispute.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.WebApi.Controllers;

[Authorize(Policy = "CustomerOnly")]
public class DisputesController : BaseController
{
    private readonly IDisputeService service;

    public DisputesController(IDisputeService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> RaiseDispute([FromBody] RaiseDisputeCommand command, CancellationToken ct)
        => ToActionResult(await service.RaiseDisputeAsync(command, ct));

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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDispute(Guid id, CancellationToken ct)
        => ToActionResult(await service.GetDisputeDetailAsync(id, ct));

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
        => ToActionResult(await service.GetSummaryStatsAsync(ct));
}
