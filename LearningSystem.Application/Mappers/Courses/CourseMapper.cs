using LearningSystem.Application.Commands.Courses;
using LearningSystem.Application.Results.Courses;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Mappers.Courses;

public class CourseMapper : ICourseMapper
{
    public CourseResult Map(Course course)
    {
        return new CourseResult
        {
            Id = course.Id,
            Title = course.Title,
            ShortDescription = course.ShortDescription,
            LongDescription = course.LongDescription,
            Category = course.Category.Name,
            Difficulty = course.Difficulty,
            CreatedBy = course.CreatedBy,
            Thumbnail = course.Thumbnail,
            CreatedAt = course.CreatedAt,
            IsPublished = course.IsPublished
        };
    }

    public IEnumerable<CourseResult> Map(IEnumerable<Course> courses)
    {
        return courses.Select(Map);
    }

    public Course Map(CreateCourseCommand command)
    {
        return new Course
        {
            Title = command.Title,
            ShortDescription = command.ShortDescription,
            LongDescription = command.LongDescription,
            CategoryId = command.CategoryId,
            Difficulty = command.Difficulty,
            CreatedBy = command.CreatedBy,
            Thumbnail = command.Thumbnail,
            IsPublished = command.IsPublished
        };
    }

    public void Map(UpdateCourseCommand command, Course course)
    {
        course.Id = command.Id;
        course.Title = command.Title;
        course.ShortDescription = command.ShortDescription;
        course.LongDescription = command.LongDescription;
        course.CategoryId = command.CategoryId;
        course.Difficulty = command.Difficulty;
        course.Thumbnail = command.Thumbnail;
        course.IsPublished = command.IsPublished;
    }
}
