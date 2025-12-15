using LearningSystem.Application.Persistence;
using LearningSystem.Domain.Entities;

namespace LearningSystem.Infrastructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly LearningSystemDbContext _context;

    public CourseRepository(LearningSystemDbContext context)
    {
        _context = context;
    }

    public void AddCourse(Course course)
    {
        _context.Courses.Add(course);
        _context.SaveChanges();
    }

    public void RemoveCourse(Course course)
    {
        _context.Courses.Remove(course);
        _context.SaveChanges();
    }

    public ICollection<Course> GetAllCourses()
    {
        return _context.Courses.ToList();
    }

    public Course? GetCourseById(int id)
    {
        return _context.Courses.FirstOrDefault(c => c.Id == id);
    }

    public void UpdateCourse(Course course)
    {
        _context.Courses.Update(course);
        _context.SaveChanges();
    }
}
