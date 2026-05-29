using DisputePortal.Application.Feature.Account.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.WebApi.Controllers;

[Authorize(Policy = "CustomerOnly")]
public class AccountsController : BaseController
{
    private readonly IBankAccountService service;

    public AccountsController(IBankAccountService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts(CancellationToken ct)
        => ToActionResult(await service.GetCustomerAccountsAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAccount(Guid id, CancellationToken ct)
        => ToActionResult(await service.GetAccountByIdAsync(id, ct));
}
