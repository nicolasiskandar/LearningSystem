using LearningSystem.Application.Commands.QuestionTypes;
using LearningSystem.Application.Results.QuestionTypes;

namespace LearningSystem.Application.Services.QuestionTypes;

public interface IQuestionTypeService
{
    Task<QuestionTypeResult?> GetQuestionTypeByIdAsync(int id);
    Task<ICollection<QuestionTypeResult>> GetQuestionTypesAsync();
    Task<QuestionTypeResult> AddQuestionTypeAsync(CreateQuestionTypeCommand command);
    Task<QuestionTypeResult> UpdateQuestionTypeAsync(UpdateQuestionTypeCommand command);
    Task DeleteQuestionTypeAsync(int id);
}