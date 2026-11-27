using HelpDesk.Api.Constants;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = UserRoles.Admin)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
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
}