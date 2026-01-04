using LearningSystem.Application.Persistence;
using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Common.Exceptions.Courses;
using LearningSystem.Application.Common.Exceptions.Users;
using LearningSystem.Application.Results.Courses;
using LearningSystem.Application.Mappers.Courses;
using LearningSystem.Application.Common.Exceptions.Categories;
using LearningSystem.Domain.Entities;
using LearningSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using LearningSystem.Application.Common.Exceptions.UserCourse;
using System.Security.Claims;
using LearningSystem.Application.Common.Exceptions;
using LearningSystem.Application.Common.Caching;

namespace LearningSystem.Application.Services.Courses;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICourseMapper _courseMapper;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IUserCourseRepository _userCourseRepository;
    private readonly ICacheService _cacheService;

    public CourseService(
        ICourseRepository courseRepository,
        ICategoryRepository categoryRepository,
        ICourseMapper courseMapper,
        UserManager<User> userManager,
        IUserRepository userRepository,
        IUserCourseRepository userCourseRepository,
        ICacheService cacheService)
    {
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
        _courseMapper = courseMapper;
        _userManager = userManager;
        _userRepository = userRepository;
        _userCourseRepository = userCourseRepository;
        _cacheService = cacheService;
    }

    public async Task EnrollUserInCourse(int userId, int courseId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new UserNotFoundException(userId);

        var course = await _courseRepository.GetCourseByIdAsync(courseId);
        if (course == null)
            throw new CourseNotFoundException(courseId);

        var userCourse = await _userCourseRepository.GetUserCourseAsync(userId, courseId);
        if (userCourse != null)
            throw new UserAlreadyEnrolledInCourseException(userId, courseId);

        var newUserCourse = new UserCourse
        {
            UserId = userId,
            CourseId = courseId
        };

        await _userCourseRepository.AddUserCourseAsync(newUserCourse);
    }

    public async Task<CourseResult> GetCourseByIdAsync(int id)
    {
        var cachedCourse = await _cacheService.GetAsync<CourseResult>($"course-{id}");
        if (cachedCourse != null)
            return cachedCourse;

        var course = await _courseRepository.GetCourseByIdAsync(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        var result = _courseMapper.Map(course);
        await _cacheService.SetAsync($"course-{id}", result);

        return result;
    }

    public async Task<IEnumerable<CourseResult>> GetCoursesAsync(int page, int pageSize)
    {
        var cachedCourses = await _cacheService.GetAsync<IEnumerable<CourseResult>>("courses-all");
        if (cachedCourses != null)
            return cachedCourses.Skip((page - 1) * pageSize).Take(pageSize);

        var courses = await _courseRepository.GetAllCoursesAsync(page, pageSize);
        var result = _courseMapper.Map(courses);
        
        return result;
    }

    public async Task<CourseResult> AddCourseAsync(CreateCourseCommand command, ClaimsPrincipal claimsPrincipal)
    {
        var creator = await _userManager.FindByIdAsync(command.CreatedBy.ToString());
        if (creator == null)
            throw new UserNotFoundException(command.CreatedBy);

        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && command.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to create this course");

        await TransformStudentToInstructor(creator);

        var category = await _categoryRepository.GetCategoryByIdAsync(command.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException(command.CategoryId);

        var course = _courseMapper.Map(command);
        course.CreatedAt = DateTime.UtcNow;

        await _courseRepository.AddCourseAsync(course);
        await _cacheService.RemoveAsync("courses-all");

        return _courseMapper.Map(course);
    }

    public async Task<CourseResult> UpdateCourseAsync(UpdateCourseCommand command, ClaimsPrincipal claimsPrincipal)
    {
        var course = await _courseRepository.GetCourseByIdAsync(command.Id);
        if (course == null)
            throw new CourseNotFoundException(command.Id);

        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());
        
        if (!isAdmin && course.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to update this course");

        var category = await _categoryRepository.GetCategoryByIdAsync(command.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException(command.CategoryId);

        _courseMapper.Map(command, course);
        await _courseRepository.UpdateCourseAsync(course);
        
        await _cacheService.RemoveAsync($"course-{command.Id}");
        await _cacheService.RemoveAsync("courses-all");

        return _courseMapper.Map(course);
    }

    public async Task DeleteCourseAsync(int id, ClaimsPrincipal claimsPrincipal)
    {
        var course = await _courseRepository.GetCourseByIdAsync(id);
        if (course == null)
            throw new CourseNotFoundException(id);
        
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UserNotFoundException("User not found");
        
        var isAdmin = claimsPrincipal.IsInRole(Roles.SuperAdmin.ToString());

        if (!isAdmin && course.CreatedBy.ToString() != userId)
            throw new ForbiddenExcception("You are not authorized to delete this course");

        await _courseRepository.RemoveCourseAsync(course);
        
        await _cacheService.RemoveAsync($"course-{id}");
        await _cacheService.RemoveAsync("courses-all");
    }

    private async Task TransformStudentToInstructor(User creator)
    {
        var isInstructor = await _userManager.IsInRoleAsync(creator, Roles.Instructor.ToString());
        if (isInstructor) return;

        var notAStudent = !await _userManager.IsInRoleAsync(creator, Roles.Student.ToString());
        if (notAStudent) return;

        var removeResult = await _userManager.RemoveFromRoleAsync(creator, Roles.Student.ToString());
        if (!removeResult.Succeeded)
            throw new InvalidOperationException("Failed to remove Student role from the user.");

        var addResult = await _userManager.AddToRoleAsync(creator, Roles.Instructor.ToString());
        if (!addResult.Succeeded)
            throw new InvalidOperationException("Failed to assign Instructor role to the user.");
    }
}
