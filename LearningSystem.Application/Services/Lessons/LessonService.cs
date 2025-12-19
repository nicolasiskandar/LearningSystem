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

namespace LearningSystem.Application.Services.Lessons;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ILessonMapper _lessonMapper;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserCourseRepository _userCourseRepository;
    private readonly ILessonCompletedRepository _lessonCompletedRepository;

    public LessonService(
        ILessonRepository lessonRepository,
        ILessonMapper lessonMapper,
        ICourseRepository courseRepository,
        IUserCourseRepository userCourseRepository,
        ILessonCompletedRepository lessonCompletedRepository)
    {
        _lessonRepository = lessonRepository;
        _lessonMapper = lessonMapper;
        _courseRepository = courseRepository;
        _userCourseRepository = userCourseRepository;
        _lessonCompletedRepository = lessonCompletedRepository;
    }

    public async Task<IEnumerable<LessonResult>> GetAllLessonsAsync()
    {
        var lessons = await _lessonRepository.GetAllLessonsAsync();
        return _lessonMapper.Map(lessons);
    }

    public async Task<LessonResult> GetLessonByIdAsync(int id)
    {
        var lesson = await _lessonRepository.GetLessonByIdAsync(id);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        return _lessonMapper.Map(lesson);
    }

    public async Task<IEnumerable<LessonResult>> GetLessonByCourseIdAsync(int courseId)
    {
        var lessons = await _lessonRepository.GetLessonsByCourseIdAsync(courseId);
        return _lessonMapper.Map(lessons);
    }

    public async Task<LessonResult> CreateLessonAsync(CreateLessonCommand command, ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isInstructor = user.IsInRole(Roles.Instructor.ToString());
        var isAdmin = user.IsInRole(Roles.SuperAdmin.ToString());

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

        return _lessonMapper.Map(lesson);
    }

    public async Task<LessonResult> UpdateLessonAsync(UpdateLessonCommand command, ClaimsPrincipal user)
    {
        var lesson = await _lessonRepository.GetLessonByIdAsync(command.Id);
        if (lesson == null)
            throw new LessonNotFoundException(command.Id);
        
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isAdmin = user.IsInRole(Roles.SuperAdmin.ToString());

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
        return _lessonMapper.Map(lesson);
    }

    public async Task DeleteLessonAsync(int id, ClaimsPrincipal user)
    {
        var lesson = await _lessonRepository.GetLessonByIdAsync(id);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isAdmin = user.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && lesson.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to delete this lesson.");

        await _lessonRepository.RemoveLessonAsync(lesson);
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
}