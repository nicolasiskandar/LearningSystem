using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface IQuestionTypeRepository
{
    Task<QuestionType?> GetQuestionTypeByIdAsync(int id);
    Task<ICollection<QuestionType>> GetAllQuestionTypesAsync();
    Task AddQuestionTypeAsync(QuestionType questionType);
    Task UpdateQuestionTypeAsync(QuestionType questionType);
    Task RemoveQuestionTypeAsync(QuestionType questionType);
}