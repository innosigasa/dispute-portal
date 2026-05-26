using DisputePortal.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace DisputePortal.Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult ToActionResult<T>(RequestResult<T> result) => result.StatusCode switch
    {
        200 => Ok(result),
        201 => StatusCode(201, result),
        204 => NoContent(),
        400 => BadRequest(result),
        401 => Unauthorized(result),
        404 => NotFound(result),
        409 => Conflict(result),
        _ => StatusCode(result.StatusCode, result)
    };
}
