using LearningSystem.Api.Dtos.Courses;
using LearningSystem.Api.Mappers.Courses;
using LearningSystem.Application.Services.Courses;
using Microsoft.AspNetCore.Mvc;

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
    public ActionResult<ICollection<CourseDto>> GetCourses()
    {
        var courses = _courseService.GetCourses();
        var dtos = _courseMapper.Map(courses);
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public ActionResult<CourseDto> GetCourseById(int id)
    {
        var course = _courseService.GetCourseById(id);
        var dto = _courseMapper.Map(course);
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<CourseDto> AddCourse([FromBody] CreateCourseDto dto)
    {
        var command = _courseMapper.Map(dto);
        var createdCourse = _courseService.AddCourse(command);
        return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CourseDto> UpdateCourse(int id, [FromBody] UpdateCourseDto dto)
    {
        var command = _courseMapper.Map(dto, id);
        var updatedCourse = _courseService.UpdateCourse(command);
        return Ok(updatedCourse);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteCourse(int id)
    {
        _courseService.DeleteCourse(id);
        return NoContent();
    }
}
