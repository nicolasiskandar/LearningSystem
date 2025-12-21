using LearningSystem.Api.Dtos.QuizAttempts;
using LearningSystem.Api.Mappers.QuizAttempts;
using LearningSystem.Application.Services.QuizAttempts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QuizAttemptsController : ControllerBase
{
    private readonly IQuizAttemptService _quizAttemptService;
    private readonly IQuizAttemptMapper _quizAttemptMapper;

    public QuizAttemptsController(IQuizAttemptService quizAttemptService, IQuizAttemptMapper quizAttemptMapper)
    {
        _quizAttemptService = quizAttemptService;
        _quizAttemptMapper = quizAttemptMapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuizAttemptDto>> GetQuizAttemptById(int id)
    {
        var quizAttempt = await _quizAttemptService.GetQuizAttemptByIdAsync(id);
        var dto = _quizAttemptMapper.Map(quizAttempt);
        return Ok(dto);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<QuizAttemptDto>>> GetQuizAttemptsByUserId(int userId)
    {
        var quizAttempts = await _quizAttemptService.GetQuizAttemptByUserIdAsync(userId);
        var dtos = _quizAttemptMapper.Map(quizAttempts);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<QuizAttemptDto>> CreateQuizAttempt([FromBody] CreateQuizAttemptDto dto)
    {
        var command = _quizAttemptMapper.Map(dto);
        
        var createdQuizAttempt = await _quizAttemptService.CreateQuizAttemptAsync(command, User);
        var resultDto = _quizAttemptMapper.Map(createdQuizAttempt);

        return CreatedAtAction(nameof(GetQuizAttemptById), new { id = createdQuizAttempt.Id }, resultDto);
    }

    [HttpPost("{id:int}/submit")]
    public async Task<ActionResult<QuizAttemptDto>> SubmitQuizAttempt(int id, [FromBody] SubmitQuizAttemptDto dto)
    {
        var command = _quizAttemptMapper.Map(dto, id);

        var submittedQuizAttempt = await _quizAttemptService.SubmitQuizAsync(command, User);
        var resultDto = _quizAttemptMapper.Map(submittedQuizAttempt);

        return Ok(resultDto);
    }
}
