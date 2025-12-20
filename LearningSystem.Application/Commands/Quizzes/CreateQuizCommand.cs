namespace LearningSystem.Application.Commands.Quizzes;

public class CreateQuizCommand
{
    public string Title { get; set; }
    public int PassingScore { get; set; }
    public int TimeLimit { get; set; }
    public int CourseId { get; set; }
    public int LessonId { get; set; }

    public CreateQuizCommand(string title, int passingScore, int timeLimit, int courseId, int lessonId)
    {
        Title = title;
        PassingScore = passingScore;
        TimeLimit = timeLimit;
        CourseId = courseId;
        LessonId = lessonId;
    }
}