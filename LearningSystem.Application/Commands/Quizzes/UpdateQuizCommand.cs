namespace LearningSystem.Application.Commands.Quizzes;

public class UpdateQuizCommand
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int PassingScore { get; set; }
    public int TimeLimit { get; set; }

    public UpdateQuizCommand(int id, string title, int passingScore, int timeLimit)
    {
        Id = id;
        Title = title;
        PassingScore = passingScore;
        TimeLimit = timeLimit;
    }
}
