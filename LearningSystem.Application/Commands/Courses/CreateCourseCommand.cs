namespace LearningSystem.Application.Commands.Courses;

public class CreateCourseCommand
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public int CategoryId { get; set; }
    public string Difficulty { get; set; }
    public int CreatedBy { get; set; }
    public string Thumbnail { get; set; }
    public bool IsPublished { get; set; }
    public CreateCourseCommand(string title, string shortDescription, string longDescription, int categoryId, string difficulty, int createdBy, string thumbnail, bool isPublished)
    {
        Title = title;
        ShortDescription = shortDescription;
        LongDescription = longDescription;
        CategoryId = categoryId;
        Difficulty = difficulty;
        CreatedBy = createdBy;
        Thumbnail = thumbnail;
        IsPublished = isPublished;
    }
}
