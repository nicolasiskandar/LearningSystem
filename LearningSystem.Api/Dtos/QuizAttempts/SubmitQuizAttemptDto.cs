namespace LearningSystem.Api.Dtos.QuizAttempts;

public class SubmitQuizAttemptDto
{
    public ICollection<QuizAttemptAnswerDto> Answers { get; set; } = [];
}
