using HelpDesk.Api.DTOs;

namespace HelpDesk.Api.Interfaces;

public interface IAuthService
{
    Task<RegisterResponseDto?>RegisterAsync(RegisterDto registerDto);
}