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

namespace LearningSystem.Application.Services.Courses;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICourseMapper _courseMapper;
    private readonly UserManager<User> _userManager;

    public CourseService(
        ICourseRepository courseRepository,
        ICategoryRepository categoryRepository,
        ICourseMapper courseMapper,
        UserManager<User> userManager)
    {
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
        _courseMapper = courseMapper;
        _userManager = userManager;
    }

    public async Task<CourseResult> GetCourseByIdAsync(int id)
    {
        var course = await _courseRepository.GetCourseByIdAsync(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        return _courseMapper.Map(course);
    }

    public async Task<IEnumerable<CourseResult>> GetCoursesAsync()
    {
        var courses = await _courseRepository.GetAllCoursesAsync();
        return _courseMapper.Map(courses);
    }

    public async Task<CourseResult> AddCourseAsync(CreateCourseCommand command)
    {
        var creator = await _userManager.FindByIdAsync(command.CreatedBy.ToString());
        if (creator == null)
            throw new UserNotFoundException(command.CreatedBy);

        if (!await _userManager.IsInRoleAsync(creator, Roles.Instructor.ToString()))
        {
            if (await _userManager.IsInRoleAsync(creator, Roles.Student.ToString()))
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(creator, Roles.Student.ToString());
                if (!removeResult.Succeeded)
                    throw new InvalidOperationException("Failed to remove Student role from the user.");
            }

            var addResult = await _userManager.AddToRoleAsync(creator, Roles.Instructor.ToString());
            if (!addResult.Succeeded)
                throw new InvalidOperationException("Failed to assign Instructor role to the user.");
        }

        var category = await _categoryRepository.GetCategoryByIdAsync(command.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException(command.CategoryId);

        var course = _courseMapper.Map(command);
        course.CreatedAt = DateTime.UtcNow;

        await _courseRepository.AddCourseAsync(course);

        return _courseMapper.Map(course);
    }


    public async Task<CourseResult> UpdateCourseAsync(UpdateCourseCommand command)
    {
        var course = await _courseRepository.GetCourseByIdAsync(command.Id);
        if (course == null)
            throw new CourseNotFoundException(command.Id);

        var category = await _categoryRepository.GetCategoryByIdAsync(command.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException(command.CategoryId);

        _courseMapper.Map(command, course);
        await _courseRepository.UpdateCourseAsync(course);

        return _courseMapper.Map(course);
    }

    public async Task DeleteCourseAsync(int id)
    {
        var course = await _courseRepository.GetCourseByIdAsync(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        await _courseRepository.RemoveCourseAsync(course);
    }
}
