using DisputePortal.Application.Feature.Lookup.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.Api.Controllers;

[Route("api/lookups")]
[Authorize]
public class LookupsController(ILookupService service) : BaseController
{
    /// <summary>Get all dispute reason options.</summary>
    [HttpGet("dispute-reasons")]
    public async Task<IActionResult> GetDisputeReasons(CancellationToken ct)
        => ToActionResult(await service.GetDisputeReasonsAsync(ct));

    /// <summary>Get all transaction category options.</summary>
    [HttpGet("transaction-categories")]
    public async Task<IActionResult> GetTransactionCategories(CancellationToken ct)
        => ToActionResult(await service.GetTransactionCategoriesAsync(ct));
}
