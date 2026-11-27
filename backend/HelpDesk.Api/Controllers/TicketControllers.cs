
using System.Security.Claims;
using HelpDesk.Api.Constants;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // Get the logged-in user's ID from the JWT token
    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(userId, out var id) ? id : 0;
    }

    // POST: api/Tickets
    // Customer creates a ticket
    [HttpPost]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<IActionResult> CreateTicket(CreateTicketDto dto)
    {
        var userId = GetCurrentUserId();

        if (userId == 0)
        {
            return Unauthorized();
        }

        var ticket = await _ticketService.CreateTicketAsync(dto, userId);

        if (ticket == null)
        {
            return BadRequest(new
            {
                message = "Invalid or inactive category."
            });
        }

        return CreatedAtAction(
            nameof(GetTicketById),
            new { id = ticket.Id },
            ticket
        );
    }

    // GET: api/Tickets/my-tickets
    // Customer views their own tickets
    [HttpGet("my-tickets")]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = GetCurrentUserId();

        if (userId == 0)
        {
            return Unauthorized();
        }

        var tickets = await _ticketService.GetMyTicketsAsync(userId);

        return Ok(tickets);
    }

    // GET: api/Tickets/assigned
    // Agent views tickets assigned to them
    [HttpGet("assigned")]
    [Authorize(Roles = UserRoles.Agent)]
    public async Task<IActionResult> GetAssignedTickets()
    {
        var agentId = GetCurrentUserId();

        if (agentId == 0)
        {
            return Unauthorized();
        }

        var tickets =
            await _ticketService.GetAssignedTicketsAsync(agentId);

        return Ok(tickets);
    }

    // GET: api/Tickets
    // Manager/Admin views all tickets
    [HttpGet]
    [Authorize(Roles = UserRoles.Manager + "," + UserRoles.Admin)]
    public async Task<IActionResult> GetAllTickets()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();

        return Ok(tickets);
    }

    // GET: api/Tickets/5
    // Users can view only tickets they are allowed to access
    [HttpGet("{id:int}")]
    [Authorize(Roles = UserRoles.Customer + "," +
                      UserRoles.Agent + "," +
                      UserRoles.Manager + "," +
                      UserRoles.Admin)]
    public async Task<IActionResult> GetTicketById(int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);

        if (ticket == null)
        {
            return NotFound(new { message = "Ticket not found." });
        }

        var userId = GetCurrentUserId();

        if (userId == 0)
        {
            return Unauthorized();
        }

        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role == UserRoles.Customer && ticket.CreatedById != userId)
        {
            return Forbid();
        }

        if (role == UserRoles.Agent && ticket.AssignedToId != userId)
        {
            return Forbid();
        }

        return Ok(ticket);
    }

    // PUT: api/Tickets/5/status
    // Only the assigned Agent can update ticket status
    [HttpPut("{id:int}/status")]
    [Authorize(Roles = UserRoles.Agent)]
    public async Task<IActionResult> UpdateTicketStatus(
        int id,
        UpdateTicketStatusDto dto)
    {
        var agentId = GetCurrentUserId();

        if (agentId == 0)
        {
            return Unauthorized();
        }

        var existingTicket =
            await _ticketService.GetTicketByIdAsync(id);

        if (existingTicket == null)
        {
            return NotFound(new { message = "Ticket not found." });
        }

        if (existingTicket.AssignedToId != agentId)
        {
            return Forbid();
        }

        var updatedTicket =
            await _ticketService.UpdateTicketStatusAsync(id, dto);

        return Ok(updatedTicket);
    }

    // PUT: api/Tickets/5/assign
    // Only Manager/Admin can assign tickets
    [HttpPut("{id:int}/assign")]
    [Authorize(Roles = UserRoles.Manager + "," + UserRoles.Admin)]
    public async Task<IActionResult> AssignTicket(
        int id,
        AssignTicketDto dto)
    {
        var ticket = await _ticketService.AssignTicketAsync(id, dto);

        if (ticket == null)
        {
            return BadRequest(new
            {
                message = "Ticket or Agent not found. Make sure the selected user has the Agent role."
            });
        }

        return Ok(ticket);
    }
}
