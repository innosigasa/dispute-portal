using DisputePortal.Application.Domain.Enums;
using DisputePortal.Application.Feature.Transaction.Requests;
using DisputePortal.Application.Feature.Transaction.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.WebApi.Controllers;

[Authorize(Policy = "CustomerOnly")]
public class TransactionsController : BaseController
{
    private readonly ITransactionService service;

    public TransactionsController(ITransactionService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? sortField = "TransactionDate",
        [FromQuery] string? sortDir = "desc",
        [FromQuery] Guid? accountId = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] TransactionCategory? category = null,
        [FromQuery] decimal? amountMin = null,
        [FromQuery] decimal? amountMax = null,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var filters = new TransactionFilters(dateFrom, dateTo, category, amountMin, amountMax, search,
            sortField ?? "TransactionDate", sortDir ?? "desc");
        return ToActionResult(await service.GetPagedAsync(accountId, filters, page, Math.Min(pageSize, 100), ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransaction(Guid id, CancellationToken ct)
        => ToActionResult(await service.GetByIdAsync(id, ct));
}
