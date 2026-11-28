using HelpDesk.Api.Constants;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var user = await _userService.CreateUserAsync(dto);

        if (user == null)
        {
            return Conflict(new
            {
                message = "A user with this email already exists."
            });
        }

        return Created("", user);
    }

    [HttpGet("agents")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> GetAgents()
    {
        var agents = await _userService.GetAgentsAsync();

        return Ok(agents);
    }
}