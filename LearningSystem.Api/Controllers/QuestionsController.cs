using LearningSystem.Api.Dtos.Questions;
using LearningSystem.Api.Mappers.Questions;
using LearningSystem.Application.Services.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSystem.Api.Controllers;

[Authorize(Roles = "Instructor,SuperAdmin")]
[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;
    private readonly IQuestionMapper _questionMapper;

    public QuestionsController(IQuestionService questionService, IQuestionMapper questionMapper)
    {
        _questionService = questionService;
        _questionMapper = questionMapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ICollection<QuestionDto>>> GetQuestions()
    {
        var questions = await _questionService.GetQuestionsAsync();
        var dtos = _questionMapper.Map(questions);
        return Ok(dtos);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuestionDto>> GetQuestionById(int id)
    {
        var question = await _questionService.GetQuestionByIdAsync(id);
        var dto = _questionMapper.Map(question);
        return Ok(dto);
    }

    [Authorize]
    [HttpGet("quiz/{quizId:int}")]
    public async Task<ActionResult<ICollection<QuestionDto>>> GetQuestionsByQuizId(int quizId)
    {
        var questions = await _questionService.GetQuestionsByQuizIdAsync(quizId);
        var dtos = _questionMapper.Map(questions);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<QuestionDto>> AddQuestion([FromBody] CreateQuestionDto dto)
    {
        var command = _questionMapper.Map(dto);
        var createdQuestion = await _questionService.AddQuestionAsync(command, User);
        return CreatedAtAction(nameof(GetQuestionById), new { id = createdQuestion.Id }, createdQuestion);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<QuestionDto>> UpdateQuestion(int id, [FromBody] UpdateQuestionDto dto)
    {
        var command = _questionMapper.Map(dto, id);
        var updatedQuestion = await _questionService.UpdateQuestionAsync(command, User);
        return Ok(updatedQuestion);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteQuestion(int id)
    {
        await _questionService.DeleteQuestionAsync(id, User);
        return NoContent();
    }
}