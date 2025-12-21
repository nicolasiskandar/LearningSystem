using LearningSystem.Application.Commands.QuizAttempts;
using LearningSystem.Application.Common.Exceptions.Answers;
using LearningSystem.Application.Common.Exceptions.QuizAttempt;
using LearningSystem.Application.Common.Exceptions.Quizzes;
using LearningSystem.Application.Common.Exceptions.UserCourse;
using LearningSystem.Application.Common.Exceptions.Users;
using LearningSystem.Application.Mappers.QuizAttempts;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.QuizAttempts;
using LearningSystem.Domain.Entities;
using System.Security.Claims;

namespace LearningSystem.Application.Services.QuizAttempts;

public class QuizAttemptService : IQuizAttemptService
{
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    private readonly IQuizAttemptAnswerRepository _quizAttemptAnswerRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserCourseRepository _userCourseRepository;
    private readonly IQuizAttemptMapper _quizAttemptMapper;

    public QuizAttemptService(
        IQuizAttemptRepository quizAttemptRepository,
        IQuizAttemptAnswerRepository quizAttemptAnswerRepository,
        IQuestionRepository questionRepository,
        IQuizRepository quizRepository,
        IUserRepository userRepository,
        IUserCourseRepository userCourseRepository,
        IQuizAttemptMapper quizAttemptMapper)
    {
        _quizAttemptRepository = quizAttemptRepository;
        _quizAttemptAnswerRepository = quizAttemptAnswerRepository;
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
        _userRepository = userRepository;
        _userCourseRepository = userCourseRepository;
        _quizAttemptMapper = quizAttemptMapper;
    }

    public async Task<QuizAttemptResult> CreateQuizAttemptAsync(CreateQuizAttemptCommand command, ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        if (!int.TryParse(userIdClaim, out int userId))
            throw new UnauthorizedAccessException("Invalid user identifier.");

        var quizUser = await _userRepository.GetUserByIdAsync(userId);
        if (quizUser == null)
            throw new UserNotFoundException($"User with id {userId} not found");

        var quiz = await _quizRepository.GetQuizByIdAsync(command.QuizId);
        if (quiz == null)
            throw new QuizNotFoundException(command.QuizId);

        var userCourse = await _userCourseRepository.GetUserCourseAsync(userId, quiz.CourseId);
        if (userCourse == null)
            throw new NotEnrolledInCourseException(userId, quiz.CourseId);

        var quizAttempt = new QuizAttempt
        {
            QuizId = command.QuizId,
            UserId = userId,
            AttemptDate = DateTime.UtcNow
        };

        await _quizAttemptRepository.AddQuizAttemptAsync(quizAttempt);
        return _quizAttemptMapper.Map(quizAttempt);
    }

    public async Task<QuizAttemptResult> GetQuizAttemptByIdAsync(int id)
    {
        var quizAttempt = await _quizAttemptRepository.GetQuizAttemptByIdAsync(id);
        if (quizAttempt == null)
            throw new QuizAttemptNotFoundException(id);

        return _quizAttemptMapper.Map(quizAttempt);
    }

    public async Task<IEnumerable<QuizAttemptResult>> GetQuizAttemptByUserIdAsync(int userId)
    {
        var quizAttempts = await _quizAttemptRepository.GetQuizAttemptsByUserIdAsync(userId);
        return _quizAttemptMapper.Map(quizAttempts);
    }

    public async Task<QuizAttemptResult> SubmitQuizAsync(SubmitQuizAttemptCommand command, ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        if (!int.TryParse(userIdClaim, out int userId))
            throw new UnauthorizedAccessException("Invalid user identifier.");

        var quizAttempt = await _quizAttemptRepository.GetQuizAttemptByIdAsync(command.Id);
        if (quizAttempt == null)
            throw new QuizAttemptNotFoundException(command.Id);

        if (quizAttempt.UserId != userId)
            throw new UnauthorizedAccessException("User can only submit their own quiz attempts.");

        var quiz = await _quizRepository.GetQuizByIdAsync(quizAttempt.QuizId);
        if (quiz == null)
            throw new QuizNotFoundException(quizAttempt.QuizId);

        var userCourse = await _userCourseRepository.GetUserCourseAsync(userId, quiz.CourseId);
        if (userCourse == null)
            throw new NotEnrolledInCourseException(userId, quiz.CourseId);

        var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizAttempt.QuizId);
        var score = 0;

        foreach (var answer in command.Answers)
        {
            var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question != null)
            {
                var submittedAnswer = question.Answers.FirstOrDefault(a => a.Id == answer.AnswerId);
                if (submittedAnswer == null)
                    throw new AnswerNotFoundException(answer.AnswerId);

                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                if (correctAnswer != null && correctAnswer.Id == answer.AnswerId)
                    score++;
            }

            await _quizAttemptAnswerRepository.AddAsync(new QuizAttemptAnswer
            {
                QuizAttemptId = command.Id,
                QuestionId = answer.QuestionId,
                AnswerId = answer.AnswerId
            });
        }

        quizAttempt.Score = score;
        await _quizAttemptRepository.UpdateQuizAttemptAsync(quizAttempt);

        return _quizAttemptMapper.Map(quizAttempt);
    }
}
