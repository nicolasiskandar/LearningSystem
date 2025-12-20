using LearningSystem.Application.Commands.Questions;
using LearningSystem.Application.Common.Exceptions;
using LearningSystem.Application.Common.Exceptions.Courses;
using LearningSystem.Application.Common.Exceptions.Lessons;
using LearningSystem.Application.Common.Exceptions.Questions;
using LearningSystem.Application.Common.Exceptions.QuestionTypes;
using LearningSystem.Application.Common.Exceptions.Quizzes;
using LearningSystem.Application.Mappers.Questions;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.Questions;
using LearningSystem.Domain.Enums;
using System.Security.Claims;

namespace LearningSystem.Application.Services.Questions;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IQuestionTypeRepository _questionTypeRepository;
    private readonly IQuestionMapper _questionMapper;

    public QuestionService(
        IQuestionRepository questionRepository,
        IQuizRepository quizRepository,
        ILessonRepository lessonRepository,
        ICourseRepository courseRepository,
        IQuestionTypeRepository questionTypeRepository,
        IQuestionMapper questionMapper)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _questionTypeRepository = questionTypeRepository;
        _questionMapper = questionMapper;
    }

    public async Task<QuestionResult> GetQuestionByIdAsync(int id)
    {
        var question = await _questionRepository.GetQuestionByIdAsync(id);
        if (question == null)
            throw new QuestionNotFoundException(id);

        return _questionMapper.Map(question);
    }

    public async Task<IEnumerable<QuestionResult>> GetQuestionsAsync()
    {
        var questions = await _questionRepository.GetAllQuestionsAsync();
        return _questionMapper.Map(questions);
    }

    public async Task<IEnumerable<QuestionResult>> GetQuestionsByQuizIdAsync(int quizId)
    {
        var quiz = await _quizRepository.GetQuizByIdAsync(quizId);
        if (quiz == null)
            throw new QuizNotFoundException(quizId);

        var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizId);
        return _questionMapper.Map(questions);
    }

    public async Task<QuestionResult> AddQuestionAsync(CreateQuestionCommand command, ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var questionType = await _questionTypeRepository.GetQuestionTypeByIdAsync(command.QuestionTypeId);
        if (questionType == null)
            throw new QuestionTypeNotFoundException(command.QuestionTypeId);

        var quiz = await _quizRepository.GetQuizByIdAsync(command.QuizId);
        if (quiz == null)
            throw new QuizNotFoundException(command.QuizId);

        var course = await _courseRepository.GetCourseByIdAsync(quiz.CourseId);
        if (course == null)
            throw new CourseNotFoundException(quiz.CourseId);

        var lesson = await _lessonRepository.GetLessonByIdAsync(quiz.LessonId);
        if (lesson == null)
            throw new LessonNotFoundException(quiz.LessonId);

        var questionOrderExists = await _questionRepository.IsQuestionOrderExistsInQuizAsync(command.QuizId, command.Order);
        if (questionOrderExists)
            throw new DuplicateQuestionOrderException(command.QuizId, command.Order);


        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && course.CreatedBy.ToString() != userId || lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to create this quesiton.");

        var question = _questionMapper.Map(command);
        await _questionRepository.AddQuestionAsync(question);

        return _questionMapper.Map(question);
    }

    public async Task<QuestionResult> UpdateQuestionAsync(UpdateQuestionCommand command, ClaimsPrincipal claimsPrincipal)
    {
        var existingQuestion = await _questionRepository.GetQuestionByIdAsync(command.Id);
        if (existingQuestion == null)
            throw new QuestionNotFoundException(command.Id);

        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var questionType = await _questionTypeRepository.GetQuestionTypeByIdAsync(command.QuestionTypeId);
        if (questionType == null)
            throw new QuestionTypeNotFoundException(command.QuestionTypeId);

        var quiz = await _quizRepository.GetQuizByIdAsync(existingQuestion.QuizId);
        if (quiz == null)
            throw new QuizNotFoundException(existingQuestion.QuizId);

        var course = await _courseRepository.GetCourseByIdAsync(quiz.CourseId);
        if (course == null)
            throw new CourseNotFoundException(quiz.CourseId);

        var lesson = await _lessonRepository.GetLessonByIdAsync(quiz.LessonId);
        if (lesson == null)
            throw new LessonNotFoundException(quiz.LessonId);

        var questionOrderExists = await _questionRepository.IsQuestionOrderExistsInQuizAsync(quiz.Id, command.Order);
        if (questionOrderExists)
            throw new DuplicateQuestionOrderException(quiz.Id, command.Order);

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && course.CreatedBy.ToString() != userId || lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to update this quesiton.");

        _questionMapper.Map(command, existingQuestion);
        await _questionRepository.UpdateQuestionAsync(existingQuestion);

        return _questionMapper.Map(existingQuestion);
    }

    public async Task DeleteQuestionAsync(int id, ClaimsPrincipal claimsPrincipal)
    {
        var question = await _questionRepository.GetQuestionByIdAsync(id);
        if (question == null)
            throw new QuestionNotFoundException(id);

        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var quiz = await _quizRepository.GetQuizByIdAsync(question.QuizId);
        if (quiz == null)
            throw new QuizNotFoundException(question.QuizId);

        var course = await _courseRepository.GetCourseByIdAsync(quiz.CourseId);
        if (course == null)
            throw new CourseNotFoundException(quiz.CourseId);

        var lesson = await _lessonRepository.GetLessonByIdAsync(quiz.LessonId);
        if (lesson == null)
            throw new LessonNotFoundException(quiz.LessonId);

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && course.CreatedBy.ToString() != userId || lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to create this question.");

        await _questionRepository.RemoveQuestionAsync(question);
    }
}