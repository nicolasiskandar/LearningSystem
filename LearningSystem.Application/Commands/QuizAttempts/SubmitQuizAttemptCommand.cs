namespace LearningSystem.Application.Commands.QuizAttempts
{
    public class SubmitQuizAttemptCommand
    {
        public int Id { get; set; }
        public ICollection<QuizAttemptAnswerCommand> Answers { get; set; } = [];
    }
}
