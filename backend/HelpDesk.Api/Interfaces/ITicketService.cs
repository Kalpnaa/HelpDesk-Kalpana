
using HelpDesk.Api.DTOs;

namespace HelpDesk.Api.Interfaces;

public interface ITicketService
{
    // Customer creates a ticket
    Task<TicketResponseDto?> CreateTicketAsync(
        CreateTicketDto dto,
        int createdById);

    // Customer views their own tickets
    Task<List<TicketResponseDto>> GetMyTicketsAsync(int userId);

    // Agent views tickets assigned to them
    Task<List<TicketResponseDto>> GetAssignedTicketsAsync(int agentId);

    // Manager/Admin views all tickets
    Task<List<TicketResponseDto>> GetAllTicketsAsync();

    // Get ticket details by ID
    Task<TicketResponseDto?> GetTicketByIdAsync(int id);

    // Update ticket status
    Task<TicketResponseDto?> UpdateTicketStatusAsync(
        int id,
        UpdateTicketStatusDto dto);

    // Assign ticket to an Agent
    Task<TicketResponseDto?> AssignTicketAsync(
        int id,
        AssignTicketDto dto);
}
