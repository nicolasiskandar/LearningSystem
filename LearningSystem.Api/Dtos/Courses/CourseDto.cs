namespace LearningSystem.Api.Dtos.Courses;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string LongDescription { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Difficulty { get; set; } = null!;
    public int CreatedBy { get; set; }
    public string Thumbnail { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsPublished { get; set; }
}
