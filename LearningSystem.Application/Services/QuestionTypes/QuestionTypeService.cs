using LearningSystem.Application.Commands.QuestionTypes;
using LearningSystem.Application.Common.Exceptions.QuestionTypes;
using LearningSystem.Application.Mappers.QuestionTypes;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.QuestionTypes;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Services.QuestionTypes;

public class QuestionTypeService : IQuestionTypeService
{
    private readonly IQuestionTypeRepository _questionTypeRepository;
    private readonly IQuestionTypeMapper _questionTypeMapper;

    public QuestionTypeService(IQuestionTypeRepository questionTypeRepository, IQuestionTypeMapper questionTypeMapper)
    {
        _questionTypeRepository = questionTypeRepository;
        _questionTypeMapper = questionTypeMapper;
    }

    public async Task<QuestionTypeResult?> GetQuestionTypeByIdAsync(int id)
    {
        var questionType = await _questionTypeRepository.GetQuestionTypeByIdAsync(id);
        return questionType == null ? null : _questionTypeMapper.Map(questionType);
    }

    public async Task<ICollection<QuestionTypeResult>> GetQuestionTypesAsync()
    {
        var questionTypes = await _questionTypeRepository.GetAllQuestionTypesAsync();
        return _questionTypeMapper.Map(questionTypes).ToList();
    }

    public async Task<QuestionTypeResult> AddQuestionTypeAsync(CreateQuestionTypeCommand command)
    {
        var questionType = _questionTypeMapper.Map(command);
        await _questionTypeRepository.AddQuestionTypeAsync(questionType);
        return _questionTypeMapper.Map(questionType);
    }

    public async Task<QuestionTypeResult> UpdateQuestionTypeAsync(UpdateQuestionTypeCommand command)
    {
        var questionType = await _questionTypeRepository.GetQuestionTypeByIdAsync(command.Id);
        if (questionType == null)
            throw new QuestionTypeNotFoundException(command.Id);

        _questionTypeMapper.Map(command, questionType);
        await _questionTypeRepository.UpdateQuestionTypeAsync(questionType);
        return _questionTypeMapper.Map(questionType);
    }

    public async Task DeleteQuestionTypeAsync(int id)
    {
        var questionType = await _questionTypeRepository.GetQuestionTypeByIdAsync(id);
        if (questionType == null)
            throw new QuestionTypeNotFoundException(id);

        await _questionTypeRepository.RemoveQuestionTypeAsync(questionType);
    }
}