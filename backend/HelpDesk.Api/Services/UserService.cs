using HelpDesk.Api.Data;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Interfaces;

namespace HelpDesk.Api.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponseDto?> CreateUserAsync(CreateUserDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var existingUser = await _context.Users.AnyAsync(u => u.Email == email);

        if (existingUser)
        {
            return null;
        }

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = email,
            Role = dto.Role
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.password
        );

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return new RegisterResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<List<AgentResponseDto>> GetAgentsAsync()
    {
        return await _context.Users
            .Where(u => u.Role == "Agent")
            .Select(u => new AgentResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            })
            .ToListAsync();
    }
    public async Task<List<UserResponseDto>> GetAllUsersAsync()
{
    return await _context.Users
        .OrderBy(user => user.Role)
        .ThenBy(user => user.FullName)
        .Select(user => new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        })
        .ToListAsync();
}
}