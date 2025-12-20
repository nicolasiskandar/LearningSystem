using LearningSystem.Application.Results.Questions;

namespace LearningSystem.Application.Results.Quizzes;

public class QuizResult
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int PassingScore { get; set; }
    public int TimeLimit { get; set; }
    public int CourseId { get; set; }
    public int LessonId { get; set; }
    public List<QuestionResult> Questions { get; set; } = new();
}