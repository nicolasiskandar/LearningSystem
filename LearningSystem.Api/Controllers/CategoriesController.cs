using LearningSystem.Api.Dtos.Categories;
using LearningSystem.Api.Mappers.Categories;
using LearningSystem.Application.Services.Categories;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

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
    public ActionResult<ICollection<CategoryDto>> GetAll()
    {
        var results = _categoryService.GetAllCategories();
        var dtos = _categoryMapper.Map(results);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public ActionResult<CategoryDto> GetById(int id)
    {
        var result = _categoryService.GetCategoryById(id);
        var dto = _categoryMapper.Map(result);
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<CategoryDto> Create([FromBody] CreateCategoryDto dto)
    {
        var command = _categoryMapper.Map(dto);
        var category = _categoryService.CreateCategory(command);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public ActionResult<CategoryDto> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        var command = _categoryMapper.Map(dto, id);
        var updatedCategory = _categoryService.UpdateCategory(command);
        return Ok(updatedCategory);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _categoryService.DeleteCategory(id);
        return NoContent();
    }
}
