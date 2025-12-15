using LearningSystem.Application.Persistence;
using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Common.Exceptions.Courses;
using LearningSystem.Application.Common.Exceptions.Users;
using LearningSystem.Application.Results.Courses;
using LearningSystem.Application.Mappers.Courses;
using LearningSystem.Application.Common.Exceptions.Categories;

namespace LearningSystem.Application.Services.Courses;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICourseMapper _courseMapper;

    public CourseService(
        ICourseRepository courseRepository,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        ICourseMapper courseMapper)
    {
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _courseMapper = courseMapper;
    }

    public CourseResult GetCourseById(int id)
    {
        var course = _courseRepository.GetCourseById(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        return _courseMapper.Map(course);
    }

    public IEnumerable<CourseResult> GetCourses()
    {
        var courses = _courseRepository.GetAllCourses();
        return _courseMapper.Map(courses);
    }

    public CourseResult AddCourse(CreateCourseCommand command)
    {
        var creator = _userRepository.GetUserById(command.CreatedBy);
        if (creator == null)
            throw new UserNotFoundException(command.CreatedBy);

        var category = _categoryRepository.GetCategoryById(command.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException(command.CategoryId);

        var course = _courseMapper.Map(command);
        course.CreatedAt = DateTime.UtcNow;

        _courseRepository.AddCourse(course);

        return _courseMapper.Map(course);
    }

    public CourseResult UpdateCourse(UpdateCourseCommand command)
    {
        var course = _courseRepository.GetCourseById(command.Id);
        if (course == null)
            throw new CourseNotFoundException(command.Id);

        var category = _categoryRepository.GetCategoryById(command.CategoryId);
        if (category == null)
            throw new CategoryNotFoundException(command.CategoryId);


        _courseMapper.Map(command, course);
        _courseRepository.UpdateCourse(course);

        return _courseMapper.Map(course);
    }

    public void DeleteCourse(int id)
    {
        var course = _courseRepository.GetCourseById(id);
        if (course == null)
            throw new CourseNotFoundException(id);

        _courseRepository.RemoveCourse(course);
    }
}
