using LearningSystem.Api.Dtos.Questions;

namespace LearningSystem.Api.Dtos.Quizzes;

public class QuizDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int PassingScore { get; set; }
    public int TimeLimit { get; set; }
    public int CourseId { get; set; }
    public int LessonId { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}