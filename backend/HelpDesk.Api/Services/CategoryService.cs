
using HelpDesk.Api.Data;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Interfaces;
using HelpDesk.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Create a new category
    public async Task<CategoryResponseDto?> CreateCategoryAsync(
        CreateCategoryDto dto)
    {
        var name = dto.Name.Trim();

        // Check whether the category name already exists
        var exists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());

        if (exists)
        {
            return null;
        }

        var category = new Category
        {
            Name = name,
            Description = dto.Description?.Trim() ?? string.Empty,
            IsActive = true
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return MapToDto(category);
    }

    // Get all active categories
    public async Task<List<CategoryResponseDto>> GetAllCategoriesAsync()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .ToListAsync();

        return categories.Select(MapToDto).ToList();
    }

    // Get a category by ID
    public async Task<CategoryResponseDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        if (category == null)
        {
            return null;
        }

        return MapToDto(category);
    }

    // Update an existing category
    public async Task<CategoryResponseDto?> UpdateCategoryAsync(
        int id,
        UpdateCategoryDto dto)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        if (category == null)
        {
            return null;
        }

        var name = dto.Name.Trim();

        // Check whether another category uses this name
        var duplicateExists = await _context.Categories.AnyAsync(
            c => c.Id != id &&
                 c.Name.ToLower() == name.ToLower());

        if (duplicateExists)
        {
            return null;
        }

        category.Name = name;
        category.Description = dto.Description?.Trim() ?? string.Empty;

        await _context.SaveChangesAsync();

        return MapToDto(category);
    }

    // Deactivate a category instead of deleting it
    public async Task<bool> DeactivateCategoryAsync(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return false;
        }

        category.IsActive = false;

        await _context.SaveChangesAsync();

        return true;
    }

    // Convert Category entity into CategoryResponseDto
    private static CategoryResponseDto MapToDto(Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }
}
