using LearningSystem.Application.Results.Answers;

namespace LearningSystem.Application.Results.Questions;

public class QuestionResult
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = null!;
    public string QuestionType { get; set; } = null!;
    public int Order { get; set; }
    public int QuizId { get; set; }
    public List<AnswerResult> Answers { get; set; } = new();
}