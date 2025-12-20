using LearningSystem.Api.Dtos.Answers;

namespace LearningSystem.Api.Dtos.Questions;

public class QuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = null!;
    public string QuestionType { get; set; } = null!;
    public int Order { get; set; }
    public int QuizId { get; set; }
    public List<AnswerDto> Answers { get; set; } = new();
}
