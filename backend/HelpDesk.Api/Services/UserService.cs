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

    public UserService(ApplicationDbContext context,IPasswordHasher<User> passwordHasher)
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
}