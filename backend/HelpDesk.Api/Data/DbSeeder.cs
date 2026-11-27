using HelpDesk.Api.Constants;
using HelpDesk.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Data;

public static class DbSeeder
{
    public static async Task SeedInitialAdminAsync(
        IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var passwordHasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasher<User>>();

        var configuration = scope.ServiceProvider
            .GetRequiredService<IConfiguration>();

        var fullName = configuration["InitialAdmin:FullName"];
        var email = configuration["InitialAdmin:Email"];
        var password = configuration["InitialAdmin:Password"];

        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Initial Admin configuration is missing.");
        }

        email = email.Trim().ToLowerInvariant();

        var adminExists = await context.Users
            .AnyAsync(u => u.Email == email);

        if (adminExists)
        {
            return;
        }

        var admin = new User
        {
            FullName = fullName,
            Email = email,
            Role = UserRoles.Admin
        };

        admin.PasswordHash = passwordHasher.HashPassword(
            admin,
            password
        );

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}