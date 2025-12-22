using LearningSystem.Api.Dtos.Quizzes;
using LearningSystem.Api.Mappers.Quizzes;
using LearningSystem.Application.Services.Quizzes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly IQuizMapper _quizMapper;

    public QuizzesController(IQuizService quizService, IQuizMapper quizMapper)
    {
        _quizService = quizService;
        _quizMapper = quizMapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<QuizDto>>> GetQuizzes()
    {
        var quizzes = await _quizService.GetQuizzesAsync();
        var dtos = _quizMapper.Map(quizzes);
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizDto>> GetQuizById(int id)
    {
        var quiz = await _quizService.GetQuizByIdAsync(id);
        var dto = _quizMapper.Map(quiz);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<QuizDto>> AddQuiz([FromBody] CreateQuizDto dto)
    {
        var command = _quizMapper.Map(dto);
        var createdQuiz = await _quizService.AddQuizAsync(command, User);
        return CreatedAtAction(nameof(GetQuizById), new { id = createdQuiz.Id }, createdQuiz);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<QuizDto>> UpdateQuiz(int id, [FromBody] UpdateQuizDto dto)
    {
        var command = _quizMapper.Map(dto, id);
        var updatedQuiz = await _quizService.UpdateQuizAsync(command, User);
        return Ok(updatedQuiz);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteQuiz(int id)
    {
        await _quizService.DeleteQuizAsync(id, User);
        return NoContent();
    }
}