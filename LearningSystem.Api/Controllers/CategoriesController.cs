using LearningSystem.Api.Dtos.Categories;
using LearningSystem.Api.Mappers.Categories;
using LearningSystem.Application.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

[Authorize(Roles = "Instructor,SuperAdmin")]
[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ICategoryMapper _categoryMapper;

    public CategoriesController(ICategoryService categoryService, ICategoryMapper categoryMapper)
    {
        _categoryService = categoryService;
        _categoryMapper = categoryMapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<CategoryDto>>> GetAll()
    {
        var results = await _categoryService.GetAllCategoriesAsync();
        var dtos = _categoryMapper.Map(results);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);
        var dto = _categoryMapper.Map(result);
        return Ok(dto);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
    {
        var command = _categoryMapper.Map(dto);
        var category = await _categoryService.CreateCategoryAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        var command = _categoryMapper.Map(dto, id);
        var updatedCategory = await _categoryService.UpdateCategoryAsync(command);
        return Ok(updatedCategory);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
}
