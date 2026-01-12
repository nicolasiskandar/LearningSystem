using LearningSystem.Application.Commands.Lessons;
using LearningSystem.Application.Common.Exceptions;
using LearningSystem.Application.Common.Exceptions.Courses;
using LearningSystem.Application.Common.Exceptions.Lessons;
using LearningSystem.Application.Mappers.Lessons;
using LearningSystem.Application.Persistence;
using LearningSystem.Application.Results.Lessons;
using LearningSystem.Domain.Entities;
using LearningSystem.Domain.Enums;
using System.Security.Claims;

using LearningSystem.Application.Common.Caching;

namespace LearningSystem.Application.Services.Lessons;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ILessonMapper _lessonMapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserCourseRepository _userCourseRepository;
    private readonly ILessonCompletedRepository _lessonCompletedRepository;
    private readonly ICacheService _cacheService;

    public LessonService(
        ILessonRepository lessonRepository,
        ILessonMapper lessonMapper,
        ICourseRepository courseRepository,
        IUserCourseRepository userCourseRepository,
        ILessonCompletedRepository lessonCompletedRepository,
        ICacheService cacheService)
    {
        _lessonRepository = lessonRepository;
        _lessonMapper = lessonMapper;
        _courseRepository = courseRepository;
        _userCourseRepository = userCourseRepository;
        _lessonCompletedRepository = lessonCompletedRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<LessonResult>> GetAllLessonsAsync()
    {
        var cachedLessons = await _cacheService.GetAsync<IEnumerable<LessonResult>>("lessons-all");
        if (cachedLessons != null)
            return cachedLessons;

        var lessons = await _lessonRepository.GetAllLessonsAsync();
        var result = _lessonMapper.Map(lessons);
        await _cacheService.SetAsync("lessons-all", result);

        return result;
    }

    public async Task<LessonResult> GetLessonByIdAsync(int id)
    {
        var cachedLesson = await _cacheService.GetAsync<LessonResult>($"lesson-{id}");
        if (cachedLesson != null)
            return cachedLesson;

        var lesson = await _lessonRepository.GetLessonByIdAsync(id);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        var result = _lessonMapper.Map(lesson);
        await _cacheService.SetAsync($"lesson-{id}", result);

        return result;
    }

    public async Task<IEnumerable<LessonResult>> GetLessonByCourseIdAsync(int courseId)
    {
        var cachedLessons = await _cacheService.GetAsync<IEnumerable<LessonResult>>($"lessons-course-{courseId}");
        if (cachedLessons != null)
            return cachedLessons;

        var lessons = await _lessonRepository.GetLessonsByCourseIdAsync(courseId);
        var result = _lessonMapper.Map(lessons);
        await _cacheService.SetAsync($"lessons-course-{courseId}", result);

        return result;
    }

    public async Task<LessonResult> CreateLessonAsync(CreateLessonCommand command, ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isInstructor = claimsPrincipal.IsInRole(Roles.Instructor.ToString());
        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isInstructor && !isAdmin)
            throw new ForbiddenExcception("User must be an instructor or admin to create a lesson.");

        var course = await _courseRepository.GetCourseByIdAsync(command.CourseId);
        if (course == null)
            throw new CourseNotFoundException(command.CourseId);

        if (command.EstimatedDuration <= 0)
            throw new InvalidLessonDurationException(command.EstimatedDuration);

        if (command.Order < 0)
            throw new ArgumentException("Order cannot be negative.");

        var existsLessonWithSameOrder = await _lessonRepository.LessonOrderExistsAsync(command.CourseId, command.Order);
        if (existsLessonWithSameOrder)
            throw new DuplicateLessonOrderException(command.CourseId, command.Order);

        var lesson = _lessonMapper.Map(command);
        lesson.CreatedBy = int.Parse(userId);
        await _lessonRepository.AddLessonAsync(lesson);

        await _cacheService.RemoveAsync("lessons-all");
        await _cacheService.RemoveAsync($"lessons-course-{command.CourseId}");

        return _lessonMapper.Map(lesson);
    }

    public async Task<LessonResult> UpdateLessonAsync(UpdateLessonCommand command, ClaimsPrincipal claimsPrincipal)
    {
        var lesson = await _lessonRepository.GetLessonByIdAsync(command.Id);
        if (lesson == null)
            throw new LessonNotFoundException(command.Id);
        
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to update this lesson.");

        if (command.EstimatedDuration <= 0)
            throw new InvalidLessonDurationException(command.EstimatedDuration);

        if (command.Order < 0)
            throw new ArgumentException("Order cannot be negative.");

        var existsLessonWithSameOrder = await _lessonRepository.LessonOrderExistsAsync(command.CourseId, command.Order);
        if (existsLessonWithSameOrder)
            throw new DuplicateLessonOrderException(command.CourseId, command.Order);

        _lessonMapper.Map(command, lesson);
        await _lessonRepository.UpdateLessonAsync(lesson);

        await _cacheService.RemoveAsync($"lesson-{lesson.Id}");
        await _cacheService.RemoveAsync("lessons-all");
        await _cacheService.RemoveAsync($"lessons-course-{lesson.CourseId}");

        return _lessonMapper.Map(lesson);
    }

    public async Task DeleteLessonAsync(int id, ClaimsPrincipal claimsPrincipal)
    {
        var lesson = await _lessonRepository.GetLessonByIdAsync(id);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to delete this lesson.");

        await _lessonRepository.RemoveLessonAsync(lesson);

        await _cacheService.RemoveAsync($"lesson-{id}");
        await _cacheService.RemoveAsync("lessons-all");
        await _cacheService.RemoveAsync($"lessons-course-{lesson.CourseId}");
    }

    public async Task MarkLessonAsCompletedAsync(int lessonId, ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        if (!int.TryParse(userId, out int userIdInt))
            throw new UnauthorizedAccessException("Invalid user ID.");

        var lesson = await _lessonRepository.GetLessonByIdAsync(lessonId);
        if (lesson == null)
            throw new LessonNotFoundException(lessonId);

        var userCourse = await _userCourseRepository.GetUserCourseAsync(userIdInt, lesson.CourseId);
        if (userCourse == null)
            throw new ForbiddenExcception("User is not enrolled in this course.");

        var lessonCompleted = await _lessonCompletedRepository.GetByUserAndLessonAsync(userIdInt, lessonId);
        if (lessonCompleted != null)
            throw new LessonAlreadyCompletedException("Lesson already marked as completed for this user.");

        var newLessonCompleted = new LessonCompleted
        {
            UserId = userIdInt,
            LessonId = lessonId,
            CompletedDate = DateTime.UtcNow
        };

        await _lessonCompletedRepository.AddAsync(newLessonCompleted);
    }

    public async Task<bool> IsLessonCompletedAsync(int lessonId, int userId)
    {
        var lessonCompleted = await _lessonCompletedRepository.GetByUserAndLessonAsync(userId, lessonId);
        return lessonCompleted != null;
    }
}