using HelpDesk.Api.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/role-test")]
[Authorize]
public class RuleTestController : ControllerBase
{
    [HttpGet("authentication")]
    public IActionResult Authenticated()
    {
        return Ok("you have accesses");
    }
     [HttpGet("customer")]
    [Authorize(Roles = UserRoles.Customer)]
    public IActionResult CustomerOnly()
    {
        return Ok("Welcome Customer!");
    }

    [HttpGet("agent")]
    [Authorize(Roles = UserRoles.Agent)]
    public IActionResult AgentOnly()
    {
        return Ok("Welcome Agent!");
    }

    [HttpGet("manager")]
    [Authorize(Roles = UserRoles.Manager)]
    public IActionResult ManagerOnly()
    {
        return Ok("Welcome Manager!");
    }

    [HttpGet("admin")]
    [Authorize(Roles = UserRoles.Admin)]
    public IActionResult AdminOnly()
    {
        return Ok("Welcome Admin!");
    }

}