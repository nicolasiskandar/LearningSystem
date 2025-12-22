using LearningSystem.Api.Dtos.Lessons;
using LearningSystem.Api.Mappers.Lessons;
using LearningSystem.Application.Services.Lessons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearningSystem.Api.Controllers;

[Authorize(Roles = "Instructor,SuperAdmin")]
[Route("api/lessons")]
[ApiController]
[EnableRateLimiting("fixed")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;
    private readonly ILessonMapper _lessonMapper;

    public LessonsController(ILessonService lessonService, ILessonMapper lessonMapper)
    {
        _lessonService = lessonService;
        _lessonMapper = lessonMapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<ICollection<LessonDto>>> GetAll()
    {
        var results = await _lessonService.GetAllLessonsAsync();
        var dtos = _lessonMapper.Map(results);
        return Ok(dtos);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<LessonDto>> GetById(int id)
    {
        var result = await _lessonService.GetLessonByIdAsync(id);
        var dto = _lessonMapper.Map(result);
        return Ok(dto);
    }

    [AllowAnonymous]
    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<ICollection<LessonDto>>> GetByCourseId(int courseId)
    {
        var results = await _lessonService.GetLessonByCourseIdAsync(courseId);
        var dtos = _lessonMapper.Map(results);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<LessonDto>> Create([FromBody] CreateLessonDto dto)
    {
        var command = _lessonMapper.Map(dto);
        var lesson = await _lessonService.CreateLessonAsync(command, User);
        return CreatedAtAction(nameof(GetById), new { id = lesson.Id }, lesson);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LessonDto>> Update(int id, [FromBody] UpdateLessonDto dto)
    {
        var command = _lessonMapper.Map(dto, id);
        var updatedLesson = await _lessonService.UpdateLessonAsync(command, User);
        return Ok(updatedLesson);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _lessonService.DeleteLessonAsync(id, User);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{lessonId}/complete")]
    public async Task<IActionResult> MarkLessonAsCompleted(int lessonId)
    {
        await _lessonService.MarkLessonAsCompletedAsync(lessonId, User);
        return Ok();
    }
}
