using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Poc.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poc.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    [HttpGet("anon")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAnonAsync()
    {
        var toDos = new List<ToDo>() {
            new(0, new Guid(), "0"),
            new(1, new Guid(), "1")
        };

        return Ok(toDos);
    }

    [HttpGet("app")]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "EntraId:Scopes:Read",
        RequiredAppPermissionsConfigurationKey = "EntraId:AppPermissions:Read"
    )]
    public async Task<IActionResult> GetAsync()
    {
        var toDos = new List<ToDo>() {
            new(2, new Guid(), "2"),
            new(3, new Guid(), "3")
        };

        return Ok(toDos);
    }

    private bool RequestCanAccessToDo(Guid userId)
    {
        return IsAppMakingRequest() || (userId == GetUserId());
    }

    private Guid GetUserId()
    {
        Guid userId;
        if (!Guid.TryParse(HttpContext.User.GetObjectId(), out userId))
        {
            throw new Exception("User ID is not valid.");
        }
        return userId;
    }

    private bool IsAppMakingRequest()
    {
        if (HttpContext.User.Claims.Any(c => c.Type == "idtyp"))
        {
            return HttpContext.User.Claims.Any(c => c.Type == "idtyp" && c.Value == "app");
        }
        else
        {
            return HttpContext.User.Claims.Any(c => c.Type == "roles") && !HttpContext.User.Claims.Any(c => c.Type == "scp");
        }
    }
}
