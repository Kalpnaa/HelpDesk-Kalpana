using HelpDesk.Api.Data;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Interfaces;
using HelpDesk.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Services;

public class AuthService : IAuthService
{
  private readonly ApplicationDbContext _context;
  private readonly IPasswordHasher<User> _passwordHasher;

  public AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
  
  public async Task<RegisterResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        string email = registerDto.Email.Trim().ToLowerInvariant();

        bool emailExists = await _context.Users.AnyAsync(u=> u.Email == email);
        if(emailExists)
        {
            return null;
        }

        var user = new User
        {
            FullName = registerDto.FullName.Trim(),
            Email = email,
            Role = "Customer",
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user,registerDto.Password);

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
