using HelpDesk.Api.DTOs;

namespace HelpDesk.Api.Interfaces;

public interface ICategoryService
{
    Task<CategoryResponseDto?> CreateCategoryAsync(CreateCategoryDto dto);

    Task<List<CategoryResponseDto>> GetAllCategoriesAsync();

    Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);

    Task<CategoryResponseDto?> UpdateCategoryAsync(
        int id,
        UpdateCategoryDto dto);

    Task<bool> DeactivateCategoryAsync(int id);
}