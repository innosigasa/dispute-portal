using DisputePortal.Application.Feature.Lookup.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.WebApi.Controllers;

[Authorize]
public class LookupsController : BaseController
{
    private readonly ILookupService service;

    public LookupsController(ILookupService service)
    {
        this.service = service;
    }

    [HttpGet("dispute-reasons")]
    public async Task<IActionResult> GetDisputeReasons(CancellationToken ct)
        => ToActionResult(await service.GetDisputeReasonsAsync(ct));

    [HttpGet("transaction-categories")]
    public async Task<IActionResult> GetTransactionCategories(CancellationToken ct)
        => ToActionResult(await service.GetTransactionCategoriesAsync(ct));
}
