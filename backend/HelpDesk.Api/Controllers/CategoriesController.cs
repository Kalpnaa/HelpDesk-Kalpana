
using HelpDesk.Api.Constants;
using HelpDesk.Api.DTOs;
using HelpDesk.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/Categories
    // Any authenticated user can view active categories
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        return Ok(categories);
    }

    // GET: api/Categories/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
        {
            return NotFound(new { message = "Category not found." });
        }

        return Ok(category);
    }

    // POST: api/Categories
    // Only Admin can create categories
    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
    {
        var category = await _categoryService.CreateCategoryAsync(dto);

        if (category == null)
        {
            return Conflict(new
            {
                message = "A category with this name already exists."
            });
        }

        return CreatedAtAction(
            nameof(GetCategoryById),
            new { id = category.Id },
            category
        );
    }

    // PUT: api/Categories/1
    // Only Admin can update categories
    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UpdateCategory(
        int id,
        UpdateCategoryDto dto)
    {
        var existingCategory =
            await _categoryService.GetCategoryByIdAsync(id);

        if (existingCategory == null)
        {
            return NotFound(new { message = "Category not found." });
        }

        var updatedCategory =
            await _categoryService.UpdateCategoryAsync(id, dto);

        if (updatedCategory == null)
        {
            return Conflict(new
            {
                message = "A category with this name already exists."
            });
        }

        return Ok(updatedCategory);
    }

    // DELETE: api/Categories/1
    // Only Admin can deactivate categories
    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeactivateCategory(int id)
    {
        var result = await _categoryService.DeactivateCategoryAsync(id);

        if (!result)
        {
            return NotFound(new { message = "Category not found." });
        }

        return Ok(new
        {
            message = "Category deactivated successfully."
        });
    }
}
