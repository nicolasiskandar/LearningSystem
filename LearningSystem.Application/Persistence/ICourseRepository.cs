using LearningSystem.Domain.Entities;

namespace LearningSystem.Application.Persistence;

public interface ICourseRepository
{
    Course? GetCourseById(int id);
    ICollection<Course> GetAllCourses();
    void AddCourse(Course course);
    void UpdateCourse(Course course);
    void RemoveCourse(Course course);
}
