using LearningSystem.Application.Commands.Quizzes;
using LearningSystem.Application.Common.Exceptions;
using LearningSystem.Application.Common.Exceptions.Courses;
using LearningSystem.Application.Common.Exceptions.Lessons;
using LearningSystem.Application.Common.Exceptions.Quizzes;
using LearningSystem.Application.Mappers.Quizzes;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.Quizzes;
using LearningSystem.Domain.Enums;
using System.Security.Claims;

using LearningSystem.Application.Common.Caching;

namespace LearningSystem.Application.Services.Quizzes;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IQuizMapper _quizMapper;
    private readonly ICacheService _cacheService;

    public QuizService(
        IQuizRepository quizRepository,
        ICourseRepository courseRepository,
        ILessonRepository lessonRepository,
        IQuizMapper quizMapper,
        ICacheService cacheService)
    {
        _quizRepository = quizRepository;
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _quizMapper = quizMapper;
        _cacheService = cacheService;
    }

    public async Task<QuizResult> GetQuizByIdAsync(int id)
    {
        var cachedQuiz = await _cacheService.GetAsync<QuizResult>($"quiz-{id}");
        if (cachedQuiz != null)
            return cachedQuiz;

        var quiz = await _quizRepository.GetQuizByIdAsync(id);
        if (quiz == null)
            throw new QuizNotFoundException(id);

        var result = _quizMapper.Map(quiz);
        await _cacheService.SetAsync($"quiz-{id}", result);

        return result;
    }

    public async Task<IEnumerable<QuizResult>> GetQuizzesAsync()
    {
        var cachedQuizzes = await _cacheService.GetAsync<IEnumerable<QuizResult>>("quizzes-all");
        if (cachedQuizzes != null)
            return cachedQuizzes;

        var quizzes = await _quizRepository.GetAllQuizzesAsync();
        var result = _quizMapper.Map(quizzes);
        await _cacheService.SetAsync("quizzes-all", result);

        return result;
    }

    public async Task<QuizResult> AddQuizAsync(CreateQuizCommand command, ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        var course = await _courseRepository.GetCourseByIdAsync(command.CourseId);
        if (course == null)
            throw new CourseNotFoundException(command.CourseId);

        var lesson = await _lessonRepository.GetLessonByIdAsync(command.LessonId);
        if (lesson == null)
            throw new LessonNotFoundException(command.LessonId);

        if (!isAdmin && course.CreatedBy.ToString() != userId || lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to create this quiz.");

        var quiz = _quizMapper.Map(command);
        await _quizRepository.AddQuizAsync(quiz);

        await _cacheService.RemoveAsync("quizzes-all");

        return _quizMapper.Map(quiz);
    }

    public async Task<QuizResult> UpdateQuizAsync(UpdateQuizCommand command, ClaimsPrincipal claimsPrincipal)
    {
        var existingQuiz = await _quizRepository.GetQuizByIdAsync(command.Id);
        if (existingQuiz == null)
            throw new QuizNotFoundException(command.Id);

        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var course = await _courseRepository.GetCourseByIdAsync(existingQuiz.CourseId);
        if (course == null)
            throw new CourseNotFoundException(existingQuiz.CourseId);

        var lesson = await _lessonRepository.GetLessonByIdAsync(existingQuiz.LessonId);
        if (lesson == null)
            throw new LessonNotFoundException(existingQuiz.LessonId);

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && course.CreatedBy.ToString() != userId || lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to update this quiz.");

        _quizMapper.Map(command, existingQuiz);
        await _quizRepository.UpdateQuizAsync(existingQuiz);

        await _cacheService.RemoveAsync($"quiz-{existingQuiz.Id}");
        await _cacheService.RemoveAsync("quizzes-all");

        return _quizMapper.Map(existingQuiz);
    }

    public async Task DeleteQuizAsync(int id, ClaimsPrincipal claimsPrincipal)
    {
        var existingQuiz = await _quizRepository.GetQuizByIdAsync(id);
        if (existingQuiz == null)
            throw new QuizNotFoundException(id);

        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var course = await _courseRepository.GetCourseByIdAsync(existingQuiz.CourseId);
        if (course == null)
            throw new CourseNotFoundException(existingQuiz.CourseId);

        var lesson = await _lessonRepository.GetLessonByIdAsync(existingQuiz.LessonId);
        if (lesson == null)
            throw new LessonNotFoundException(existingQuiz.LessonId);

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && course.CreatedBy.ToString() != userId || lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to delete this quiz.");

        await _quizRepository.RemoveQuizAsync(existingQuiz);
        
        await _cacheService.RemoveAsync($"quiz-{id}");
        await _cacheService.RemoveAsync("quizzes-all");
    }
}