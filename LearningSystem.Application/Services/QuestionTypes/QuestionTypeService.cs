using LearningSystem.Application.Commands.QuestionTypes;
using LearningSystem.Application.Common.Exceptions.QuestionTypes;
using LearningSystem.Application.Mappers.QuestionTypes;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.QuestionTypes;

using LearningSystem.Application.Common.Caching;

namespace LearningSystem.Application.Services.QuestionTypes;

public class QuestionTypeService : IQuestionTypeService
{
    private readonly IQuestionTypeRepository _questionTypeRepository;
    private readonly IQuestionTypeMapper _questionTypeMapper;
    private readonly ICacheService _cacheService;

    public QuestionTypeService(
        IQuestionTypeRepository questionTypeRepository, 
        IQuestionTypeMapper questionTypeMapper,
        ICacheService cacheService)
    {
        _questionTypeRepository = questionTypeRepository;
        _questionTypeMapper = questionTypeMapper;
        _cacheService = cacheService;
    }

    public async Task<QuestionTypeResult?> GetQuestionTypeByIdAsync(int id)
    {
        var cachedQuestionType = await _cacheService.GetAsync<QuestionTypeResult>($"questiontype-{id}");
        if (cachedQuestionType != null)
            return cachedQuestionType;

        var questionType = await _questionTypeRepository.GetQuestionTypeByIdAsync(id);
        if (questionType == null)
            return null;

        var result = _questionTypeMapper.Map(questionType);
        await _cacheService.SetAsync($"questiontype-{id}", result);

        return result;
    }

    public async Task<ICollection<QuestionTypeResult>> GetQuestionTypesAsync()
    {
        var cachedQuestionTypes = await _cacheService.GetAsync<ICollection<QuestionTypeResult>>("questiontypes-all");
        if (cachedQuestionTypes != null)
            return cachedQuestionTypes;

        var questionTypes = await _questionTypeRepository.GetAllQuestionTypesAsync();
        var result = _questionTypeMapper.Map(questionTypes).ToList();
        await _cacheService.SetAsync("questiontypes-all", result);

        return result;
    }

    public async Task<QuestionTypeResult> AddQuestionTypeAsync(CreateQuestionTypeCommand command)
    {
        var questionType = _questionTypeMapper.Map(command);
        await _questionTypeRepository.AddQuestionTypeAsync(questionType);
        
        await _cacheService.RemoveAsync("questiontypes-all");

        return _questionTypeMapper.Map(questionType);
    }

    public async Task<QuestionTypeResult> UpdateQuestionTypeAsync(UpdateQuestionTypeCommand command)
    {
        var questionType = await _questionTypeRepository.GetQuestionTypeByIdAsync(command.Id);
        if (questionType == null)
            throw new QuestionTypeNotFoundException(command.Id);

        _questionTypeMapper.Map(command, questionType);
        await _questionTypeRepository.UpdateQuestionTypeAsync(questionType);
        
        await _cacheService.RemoveAsync($"questiontype-{command.Id}");
        await _cacheService.RemoveAsync("questiontypes-all");

        return _questionTypeMapper.Map(questionType);
    }

    public async Task DeleteQuestionTypeAsync(int id)
    {
        var questionType = await _questionTypeRepository.GetQuestionTypeByIdAsync(id);
        if (questionType == null)
            throw new QuestionTypeNotFoundException(id);

        await _questionTypeRepository.RemoveQuestionTypeAsync(questionType);

        await _cacheService.RemoveAsync($"questiontype-{id}");
        await _cacheService.RemoveAsync("questiontypes-all");
    }
}