
using HelpDesk.Api.Data;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Enums;
using HelpDesk.Api.Interfaces;
using HelpDesk.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Services;

public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;

    public TicketService(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1. Customer creates a ticket
    public async Task<TicketResponseDto?> CreateTicketAsync(
        CreateTicketDto dto,
        int createdById)
    {
        // Check whether the selected category exists and is active
        var categoryExists = await _context.Categories.AnyAsync(
            c => c.Id == dto.CategoryId && c.IsActive);

        if (!categoryExists)
        {
            return null;
        }

        // Generate a unique ticket number
        var ticketNumber =
            $"HD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        var ticket = new Ticket
        {
            TicketNumber = ticketNumber,
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            CategoryId = dto.CategoryId,
            Priority = dto.Priority,
            Status = TicketStatus.New,
            CreatedById = createdById,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return await GetTicketByIdAsync(ticket.Id);
    }

    // 2. Customer views their own tickets
    public async Task<List<TicketResponseDto>> GetMyTicketsAsync(int userId)
    {
        var tickets = await _context.Tickets
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .Where(t => t.CreatedById == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tickets.Select(MapToDto).ToList();
    }

    // 3. Agent views tickets assigned to them
    public async Task<List<TicketResponseDto>> GetAssignedTicketsAsync(
        int agentId)
    {
        var tickets = await _context.Tickets
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .Where(t => t.AssignedToId == agentId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tickets.Select(MapToDto).ToList();
    }

    // 4. Manager/Admin views all tickets
    public async Task<List<TicketResponseDto>> GetAllTicketsAsync()
    {
        var tickets = await _context.Tickets
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tickets.Select(MapToDto).ToList();
    }

    // 5. Get ticket details by ID
    public async Task<TicketResponseDto?> GetTicketByIdAsync(int id)
    {
        var ticket = await _context.Tickets
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
        {
            return null;
        }

        return MapToDto(ticket);
    }

    // 6. Update ticket status
    public async Task<TicketResponseDto?> UpdateTicketStatusAsync(
        int id,
        UpdateTicketStatusDto dto)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Category)
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
        {
            return null;
        }

        ticket.Status = dto.Status;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(ticket);
    }

    // 7. Manager/Admin assigns a ticket to an Agent
    public async Task<TicketResponseDto?> AssignTicketAsync(
        int id,
        AssignTicketDto dto)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Category)
            .Include(t => t.CreatedBy)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
        {
            return null;
        }

        // Only users with the Agent role can be assigned tickets
        var agent = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == dto.AssignedToId &&
                 u.Role == "Agent");

        if (agent == null)
        {
            return null;
        }

        ticket.AssignedToId = agent.Id;
        ticket.AssignedTo = agent;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToDto(ticket);
    }

    // Convert Ticket entity into TicketResponseDto
    private static TicketResponseDto MapToDto(Ticket ticket)
    {
        return new TicketResponseDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Title = ticket.Title,
            Description = ticket.Description,
            CategoryId = ticket.CategoryId,
            CategoryName = ticket.Category.Name,
            Priority = ticket.Priority,
            Status = ticket.Status,
            CreatedById = ticket.CreatedById,
            CreatedByName = ticket.CreatedBy.FullName,
            AssignedToId = ticket.AssignedToId,
            AssignedToName = ticket.AssignedTo?.FullName,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt
        };
    }
}
