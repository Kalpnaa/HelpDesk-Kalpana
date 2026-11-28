using HelpDesk.Api.DTOs;

namespace HelpDesk.Api.Interfaces;

public interface IUserService
{
    Task<RegisterResponseDto?> CreateUserAsync(CreateUserDto dto);

    Task<List<AgentResponseDto>> GetAgentsAsync();
}