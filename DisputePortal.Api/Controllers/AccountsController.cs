using DisputePortal.Application.Feature.Account.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.Api.Controllers;

[Route("api/accounts")]
[Authorize(Policy = "CustomerOnly")]
public class AccountsController(IBankAccountService service) : BaseController
{
    /// <summary>Get all bank accounts for the authenticated customer.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAccounts(CancellationToken ct)
        => ToActionResult(await service.GetCustomerAccountsAsync(ct));

    /// <summary>Get a single bank account by ID for the authenticated customer.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAccount(Guid id, CancellationToken ct)
        => ToActionResult(await service.GetAccountByIdAsync(id, ct));
}
