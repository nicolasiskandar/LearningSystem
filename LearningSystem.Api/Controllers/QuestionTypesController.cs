using LearningSystem.Api.Dtos.QuestionTypes;
using LearningSystem.Api.Mappers.QuestionTypes;
using LearningSystem.Application.Services.QuestionTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearningSystem.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("fixed")]
public class QuestionTypesController : ControllerBase
{
    private readonly IQuestionTypeService _questionTypeService;
    private readonly IQuestionTypeMapper _questionTypeMapper;

    public QuestionTypesController(IQuestionTypeService questionTypeService, IQuestionTypeMapper questionTypeMapper)
    {
        _questionTypeService = questionTypeService;
        _questionTypeMapper = questionTypeMapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<QuestionTypeDto>>> GetQuestionTypes()
    {
        var questionTypeResults = await _questionTypeService.GetQuestionTypesAsync();
        return Ok(_questionTypeMapper.Map(questionTypeResults));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuestionTypeDto>> GetQuestionTypeById(int id)
    {
        var questionTypeResult = await _questionTypeService.GetQuestionTypeByIdAsync(id);
        return Ok(_questionTypeMapper.Map(questionTypeResult));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPost]
    public async Task<ActionResult<QuestionTypeDto>> AddQuestionType([FromBody] CreateQuestionTypeDto dto)
    {
        var command = _questionTypeMapper.Map(dto);
        var createdQuestionTypeResult = await _questionTypeService.AddQuestionTypeAsync(command);
        var createdQuestionTypeDto = _questionTypeMapper.Map(createdQuestionTypeResult);
        return CreatedAtAction(nameof(GetQuestionTypeById), new { id = createdQuestionTypeDto.Id }, createdQuestionTypeDto);
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<QuestionTypeDto>> UpdateQuestionType(int id, [FromBody] UpdateQuestionTypeDto dto)
    {
        var command = _questionTypeMapper.Map(dto, id);
        var updatedQuestionTypeResult = await _questionTypeService.UpdateQuestionTypeAsync(command);
        return Ok(_questionTypeMapper.Map(updatedQuestionTypeResult));
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteQuestionType(int id)
    {
        await _questionTypeService.DeleteQuestionTypeAsync(id);
        return NoContent();
    }
}