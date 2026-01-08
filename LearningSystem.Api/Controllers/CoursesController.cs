using LearningSystem.Api.Dtos.Courses;
using LearningSystem.Api.Mappers.Courses;
using LearningSystem.Application.Services.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearningSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ICourseMapper _courseMapper;

    public CoursesController(ICourseService courseService, ICourseMapper courseMapper)
    {
        _courseService = courseService;
        _courseMapper = courseMapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<CourseDto>>> GetCourses([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] int? categoryId = null)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var courses = await _courseService.GetCoursesAsync(page, pageSize, categoryId);
        var dtos = _courseMapper.Map(courses);
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        var dto = _courseMapper.Map(course);
        return Ok(dto);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CourseDto>> AddCourse([FromBody] CreateCourseDto dto)
    {
        var command = _courseMapper.Map(dto);
        var createdCourse = await _courseService.AddCourseAsync(command, User);
        return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
    {
        var command = _courseMapper.Map(dto, id);
        var updatedCourse = await _courseService.UpdateCourseAsync(command, User);
        return Ok(updatedCourse);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        await _courseService.DeleteCourseAsync(id, User);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{courseId:int}/enroll")]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        if (!int.TryParse(userIdClaim.Value, out int userId))
            throw new UnauthorizedAccessException("Invalid user ID.");

        await _courseService.EnrollUserInCourse(userId, courseId);
        return Ok();
    }
}
